using System;
using System.Collections;
using System.Data.OleDb;
using System.IO;
using System.Net.Http.Headers;
using UnityEditor;
using UnityEngine;

public class LevelObject : MonoBehaviour {

    [Tooltip("The BlockGrid object for this level")]
    public BlockGrid blockGrid;

    [Tooltip("The LevelBlockSet objects used to build this level")]
    public LevelBlockSet[] levelBlockSets;

    [Tooltip("The LevelDecorationSet objects used to build this level")]
    public LevelDecorationSet[] levelDecorationSets;

    [Tooltip("Information about transitions in this level")]
    public LevelTransitionInfo[] levelTransitionInfo;

    [Tooltip("Lighting objects in this level")]
    public LightingFixture[] lightingFixtures;

    [Tooltip("Music clip to be played in this level")]
    public AudioClip music;

    [Tooltip("Cutscenes that might play in this level")]
    public CutsceneInfo[] cutsceneInfo;

    [Tooltip("UI elements present in this level")]
    public GameObject[] UIElements;

    // The graph used to store information about where the player can move
    public PathfindingGraph pathfindingGraph;

    // The pathfinder that gives the shortest path the player can take to a location
    public AStarPathfinding pathfinder;


    // The block type at each location on the grid
    [HideInInspector]
    public BlockGrid.StandardBlockType[] blockTypes;

    // The block class objects for each of the blocks on the grid
    [HideInInspector]
    public Block[] blocks;

    // The prefab objects to be spawned for each block in the grid
    [HideInInspector]
    public GameObject[] blockPrefabs;

    // Each of the levels that can be reached from this level
    [HideInInspector]
    public GameObject[] adjacentLevels;

    
    // The x, y, and z sizes of the block grid in this level
    [HideInInspector]
    public int xSize, ySize, zSize;

    // Has execution order 1 (runs after all other start functions have run)
    private void Start() {
        LoadInstance();
    }

    // Load things that need to be done after level is made active
    public void LoadInstance() {
        LoadPathfinding();
        SetupInteractables();
    }

    // Destroy all children objects of the level
    public void DestroyLevel() {
        while (transform.childCount > 0) {
            foreach (Transform child in transform) {
                DestroyImmediate(child.gameObject);
            }
        }
        adjacentLevels = null;
    }

    // Pre-load all of the block objects in the level
    public IEnumerator LoadLevel() {
        DestroyLevel();
        LoadLevelData();
        yield break;
    }

    public void LoadLevelData() {

        xSize = blockGrid.xSize;
        ySize = blockGrid.ySize;
        zSize = blockGrid.zSize;

        blocks = new Block[xSize * ySize * zSize];
        blockTypes = new BlockGrid.StandardBlockType[xSize * ySize * zSize];
        blockPrefabs = new GameObject[xSize * ySize * zSize];

        // Load block info from BlockGrid
        LoadBlockRects(blockGrid.blockRects, Vector3Int.zero);
        LoadDecorationRects(blockGrid.decorationRects, Vector3Int.zero);

        // Load block info from set pieces
        foreach (BlockGrid.SetPieceInstance setPiece in blockGrid.setPieces) {
            LoadBlockRects(setPiece.setPiece.blockRects, setPiece.cornerLocation);
            LoadDecorationRects(setPiece.setPiece.decorationRects, setPiece.cornerLocation);
            LoadInteractables(setPiece.setPiece.interactables, setPiece.cornerLocation);
        }

        // Load interactable object info
        LoadInteractables(blockGrid.interactables, Vector3Int.zero);

        // Use loaded block info to instantiate block objects
        LoadBlocks();

        // Load transition info into door objects
        LoadTransitions();

        // Instantiate lighting objects
        LoadLighting();

        // Instantiate UI elements
        if (UIElements == null) return;
        foreach (GameObject element in UIElements) {
            PrefabUtility.InstantiatePrefab(element, transform);
        }
    }

    public void LoadBlocks() {
        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++) {
                for (int z = 0; z < zSize; z++) {
                    int index = x + y * xSize + z * xSize * ySize;
                    Vector3Int position = new Vector3Int(x, y, z);

                    // If the block is empty and the block below it is also empty, skip it
                    if (blockTypes[index] == BlockGrid.StandardBlockType.Empty) {
                        if (y == 0 || blockTypes[index - xSize] == BlockGrid.StandardBlockType.Empty) {
                            continue;
                        } else {
                            blockPrefabs[index] = levelBlockSets[0].blockPrefabs[0];
                        }
                    }

                    GameObject prefab = blockPrefabs[index];

                    // If the prefab doesn't exist, skip it
                    if (prefab == null) continue;

                    // If the level has been destroyed, abort loading
                    if (gameObject == null) return;

                    // Spawn the block and set its parent to the level
                    GameObject obj = (GameObject) PrefabUtility.InstantiatePrefab(prefab, transform);
                    obj.transform.position += position;
                    obj.transform.rotation = prefab.transform.rotation;

                    Block block = obj.GetComponent<Block>();
                    block.gridLocation = position;

                    // If the block takes up multiple grid locations, make sure they all point to the block
                    for (int xl = block.bottomCorner.x; xl < block.topCorner.x; xl++) {
                        for (int yl = block.bottomCorner.y; yl < block.topCorner.y; yl++) {
                            for (int zl = block.bottomCorner.z; zl < block.topCorner.z; zl++) {
                                blocks[xl + yl * xSize + zl * xSize * ySize + index] = block;
                            }
                        }
                    }
                }
            }
        }
    }

    public void LoadPathfinding() {
        pathfindingGraph = new PathfindingGraph();
        pathfindingGraph.InitializeGraph(this);
        pathfinder = new AStarPathfinding(pathfindingGraph);
    }

    public void LoadBlockRects(BlockGrid.BlockRect[] blockRects, Vector3Int cornerLocation) {
        for (int i = 0; i < blockRects.Length; i++) {
            int xMin = (int)Math.Min(blockRects[i].corner0.x, blockRects[i].corner1.x);
            int xMax = (int)Math.Max(blockRects[i].corner0.x, blockRects[i].corner1.x);
            int yMin = (int)Math.Min(blockRects[i].corner0.y, blockRects[i].corner1.y);
            int yMax = (int)Math.Max(blockRects[i].corner0.y, blockRects[i].corner1.y);
            int zMin = (int)Math.Min(blockRects[i].corner0.z, blockRects[i].corner1.z);
            int zMax = (int)Math.Max(blockRects[i].corner0.z, blockRects[i].corner1.z);

            for (int x = xMin; x < xMax; x++) {
                for (int y = yMin; y < yMax; y++) {
                    for (int z = zMin; z < zMax; z++) {
                        int index = (x + cornerLocation.x) + (y + cornerLocation.y) * xSize + (z + cornerLocation.z) * xSize * ySize;

                        GameObject prefab = levelBlockSets[blockRects[i].prefabSetNumber].blockPrefabs[(int)blockRects[i].blockType];
                        Block block = prefab.GetComponent<Block>();

                        for (int xl = block.bottomCorner.x; xl < block.topCorner.x; xl++) {
                            for (int yl = block.bottomCorner.y; yl < block.topCorner.y; yl++) {
                                for (int zl = block.bottomCorner.z; zl < block.topCorner.z; zl++) {
                                    blockPrefabs[xl + yl * xSize + zl * xSize * ySize + index] = null;
                                    blockTypes[xl + yl * xSize + zl * xSize * ySize + index] = blockRects[i].blockType;
                                }
                            }
                        }

                        blockPrefabs[index] = prefab;
                    }
                }
            }
        }
    }

    public void LoadDecorationRects(BlockGrid.DecorationRect[] decorationRects, Vector3Int cornerLocation) {
        for (int i = 0; i < decorationRects.Length; i++) {
            int xMin = (int)Math.Min(decorationRects[i].corner0.x, decorationRects[i].corner1.x);
            int xMax = (int)Math.Max(decorationRects[i].corner0.x, decorationRects[i].corner1.x);
            int yMin = (int)Math.Min(decorationRects[i].corner0.y, decorationRects[i].corner1.y);
            int yMax = (int)Math.Max(decorationRects[i].corner0.y, decorationRects[i].corner1.y);
            int zMin = (int)Math.Min(decorationRects[i].corner0.z, decorationRects[i].corner1.z);
            int zMax = (int)Math.Max(decorationRects[i].corner0.z, decorationRects[i].corner1.z);

            for (int x = xMin; x < xMax; x++) {
                for (int y = yMin; y < yMax; y++) {
                    for (int z = zMin; z < zMax; z++) {
                        int index = (x + cornerLocation.x) + (y + cornerLocation.y) * xSize + (z + cornerLocation.z) * xSize * ySize;

                        GameObject prefab = levelDecorationSets[decorationRects[i].decorationSetNumber].decorationPrefabs[decorationRects[i].decorationIndex];
                        Block block = prefab.GetComponent<Block>();

                        for (int xl = block.bottomCorner.x; xl < block.topCorner.x; xl++) {
                            for (int yl = block.bottomCorner.y; yl < block.topCorner.y; yl++) {
                                for (int zl = block.bottomCorner.z; zl < block.topCorner.z; zl++) {
                                    blockPrefabs[xl + yl * xSize + zl * xSize * ySize + index] = null;
                                    blockTypes[xl + yl * xSize + zl * xSize * ySize + index] = BlockGrid.StandardBlockType.Decoration;
                                }
                            }
                        }

                        blockPrefabs[index] = prefab;
                        blockTypes[index] = BlockGrid.StandardBlockType.Decoration;
                    }
                }
            }
        }
    }

    public void LoadInteractables(BlockGrid.InteractableInfo[] interactables, Vector3Int cornerLocation) {
        foreach (BlockGrid.InteractableInfo info in interactables) {
            Vector3Int position = info.position + cornerLocation;
            int index = position.x + position.y * xSize + position.z * xSize * ySize;
            int x = position.x;
            int y = position.y;
            int z = position.z;

            GameObject prefab = info.objectPrefab;
            Block block = prefab.GetComponent<Block>();

            for (int xl = block.bottomCorner.x; xl < block.topCorner.x; xl++) {
                for (int yl = block.bottomCorner.y; yl < block.topCorner.y; yl++) {
                    for (int zl = block.bottomCorner.z; zl < block.topCorner.z; zl++) {
                        blockPrefabs[xl + yl * xSize + zl * xSize * ySize + index] = null;
                        blockTypes[xl + yl * xSize + zl * xSize * ySize + index] = BlockGrid.StandardBlockType.Interactable;
                    }
                }
            }

            blockPrefabs[index] = prefab;
            blockTypes[index] = BlockGrid.StandardBlockType.Interactable;
        }
    }

    public void LoadTransitions() {
        for (int i = 0; i < levelTransitionInfo.Length; i++) {
            LevelTransitionInfo t = levelTransitionInfo[i];
            Door door = GetBlock(Vector3Int.RoundToInt(t.doorLocation)).GetComponent<Door>();
            if (door == null) continue;
            door.nextLevelIndex = i;
        }
    }

    public void LoadLighting() {
        foreach (LightingFixture light in lightingFixtures) {
            GameObject l = (GameObject) PrefabUtility.InstantiatePrefab(light.lightingPrefab, transform);
            l.transform.position = light.position;
            l.transform.rotation = light.lightingPrefab.transform.rotation;
        }
    }

    public void SetupInteractables() {
        foreach (BlockGrid.InteractableInfo info in blockGrid.interactables) {
            Block obj = GetBlock(info.position);
            obj.GetComponent<InteractableObject>().Setup(info.stateIndex, info.targetPositions, Vector3Int.zero);
        }
        foreach (BlockGrid.SetPieceInstance setPiece in blockGrid.setPieces) {
            foreach (BlockGrid.InteractableInfo info in setPiece.setPiece.interactables) {
                Block obj = GetBlock(info.position + setPiece.cornerLocation);
                obj.GetComponent<InteractableObject>().Setup(info.stateIndex, info.targetPositions, setPiece.cornerLocation);
            }
        }
    }

    public Block GetBlock(Vector3Int location) {
        return blocks[location.x + location.y * xSize + location.z * xSize * ySize];
    }

    public void SetBlock(Vector3Int location, Block block) {
        blocks[location.x + location.y * xSize + location.z * xSize * ySize] = block;
    }

    public void SwapObjects(Vector3Int location1, Vector3Int location2) {
        Block temp = blocks[location1.x + location1.y * xSize + location1.z * xSize * ySize];
        blocks[location1.x + location1.y * xSize + location1.z * xSize * ySize] = blocks[location2.x + location2.y * xSize + location2.z * xSize * ySize];
        blocks[location2.x + location2.y * xSize + location2.z * xSize * ySize] = temp;
    }

    public void CreateBlockRect(Vector3 corner0, Vector3 corner1, BlockGrid.StandardBlockType type, int prefabSetNumber = 0) {
        int xMin = (int)Math.Min(corner0.x, corner1.x);
        int xMax = (int)Math.Max(corner0.x, corner1.x);
        int yMin = (int)Math.Min(corner0.y, corner1.y);
        int yMax = (int)Math.Max(corner0.y, corner1.y);
        int zMin = (int)Math.Min(corner0.z, corner1.z);
        int zMax = (int)Math.Max(corner0.z, corner1.z);

        for (int x = xMin; x < xMax; x++) {
            for (int y = yMin; y < yMax; y++) {
                for (int z = zMin; z < zMax; z++) {
                    int index = x + y * xSize + z * xSize * ySize;
                    Vector3 position = new Vector3(x, y, z);
                    if (type == BlockGrid.StandardBlockType.Surface) blockTypes[index] = BlockGrid.StandardBlockType.Empty;
                    else blockTypes[index] = type;
                    
                    GameObject prefab = levelBlockSets[prefabSetNumber].blockPrefabs[(int)blockTypes[index]];
                    if (prefab == null) continue;
                    if (gameObject == null) return;

                    GameObject obj = (GameObject) PrefabUtility.InstantiatePrefab(prefab, transform);
                    obj.transform.position += position;
                    obj.transform.rotation = prefab.transform.rotation;
                    obj.GetComponent<Block>().gridLocation = new Vector3Int(x, y, z);

                    if (type == BlockGrid.StandardBlockType.Empty) obj.GetComponent<Empty>().UpdatePathfinding();
                    if (type == BlockGrid.StandardBlockType.Surface) {
                        obj.GetComponent<Empty>().forceSurface = true;
                        obj.GetComponent<Empty>().UpdatePathfinding();
                    }

                    if (blocks[index] != null) {
                        blocks[index] = obj.GetComponent<Block>();
                        pathfindingGraph.UpdateNode(Vector3Int.RoundToInt(position), obj.GetComponent<Block>());
                    } else {
                        blocks[index] = obj.GetComponent<Block>();
                        pathfindingGraph.CreateNode(Vector3Int.RoundToInt(position));
                    }
                }
            }
        }
    }

    public void MoveBlock(Vector3Int oldPos, Vector3Int newPos) {
        Block block = GetBlock(oldPos);
        if (block == null) return;

        GameObject emptyPrefab = levelBlockSets[0].blockPrefabs[0];

        Block oldBlock = block.containedBlock;
        if (oldBlock != null) {
            SetBlock(oldPos, block.containedBlock);
            if (oldBlock.GetComponent<Empty>()) {
                Empty empty = oldBlock.GetComponent<Empty>();
                empty.UpdatePathfinding();
            }
        } else {
            GameObject emptyObj = (GameObject) PrefabUtility.InstantiatePrefab(emptyPrefab, transform);
            emptyObj.transform.position += oldPos;
            emptyObj.transform.rotation = emptyPrefab.transform.rotation;

            Empty empty = emptyObj.GetComponent<Empty>();
            empty.gridLocation = oldPos;
            empty.UpdatePathfinding();

            SetBlock(oldPos, empty);
            oldBlock = empty;
        }
        block.gridLocation = newPos;

        Block oldEmpty = GetBlock(newPos);
        block.containedBlock = oldEmpty;
        oldEmpty.enabled = false;

        SetBlock(newPos, block);

        pathfindingGraph.UpdateNode(newPos, block);
        pathfindingGraph.UpdateNode(oldPos, oldBlock);

        Vector3Int above = oldPos;
        above.y += 1;
        Block aboveEmpty = GetBlock(above);
        if (aboveEmpty != null && IsEmpty(aboveEmpty)) {
            Destroy(aboveEmpty.gameObject);
            SetBlock(above, null);
            pathfindingGraph.UpdateNode(above, aboveEmpty);
        }

        Vector3Int newAbove = newPos;
        newAbove.y += 1;
        Empty newEmpty = null;
        if (block.surfaceAbove && GetBlock(newAbove) == null) {
            GameObject newEmptyObj = (GameObject) PrefabUtility.InstantiatePrefab(emptyPrefab, transform);
            newEmptyObj.transform.position += newAbove;
            newEmptyObj.transform.rotation = emptyPrefab.transform.rotation;

            newEmpty = newEmptyObj.GetComponent<Empty>();
            newEmpty.gridLocation = newAbove;

            newEmpty.forceSurface = true;
            newEmpty.UpdatePathfinding();

            SetBlock(newAbove, newEmpty);
            pathfindingGraph.UpdateNode(newAbove, newEmpty);
        }

        
    }

    public bool IsEmpty(Block b) {
        if (b.gameObject.GetComponent<Empty>()) {
            return true;
        } else {
            return false;
        }
    }

    public void TranslateLevel(TranslateParameters parameters) {
        if (parameters.blockRects) {
            foreach (BlockGrid.BlockRect item in blockGrid.blockRects) {
                item.corner0 += parameters.distance;
                item.corner1 += parameters.distance;
            }
        }

        if (parameters.decorationRects) {
            foreach (BlockGrid.DecorationRect item in blockGrid.decorationRects) {
                item.corner0 += parameters.distance;
                item.corner1 += parameters.distance;
            }
        }

        if (parameters.interactables) {
            foreach (BlockGrid.InteractableInfo item in blockGrid.interactables) {
                item.position += parameters.distance;
                for (int i = 0; i < item.targetPositions.Length; i++) {
                    item.targetPositions[i] += parameters.distance;
                }
            }
        }

        if (parameters.lightingFixtures) {
            foreach (LightingFixture item in lightingFixtures) {
                item.position += parameters.distance;
            }
        }

        if (parameters.setPieces) {
            foreach (BlockGrid.SetPieceInstance item in blockGrid.setPieces) {
                item.cornerLocation += parameters.distance;
            }
        }

        if (parameters.levelTransitions) {
            foreach (LevelTransitionInfo item in levelTransitionInfo) {
                item.doorLocation += parameters.distance;
            }
        }
    }

    public void PlayCutsceneIndex(int index) {
        CutsceneManager.instance.PlaySimpleCutscene(cutsceneInfo[index].camera, cutsceneInfo[index].length, cutsceneInfo[index].fadeMusic, cutsceneInfo[index].suspendInput);
    }

    [Serializable]
    public class LevelTransitionInfo {
        public GameObject level;
        public Vector3 doorLocation;
        public Vector3 playerSpawnLocation;
        public Quaternion playerSpawnRotation;
    }

    [Serializable]
    public class LightingFixture {
        public GameObject lightingPrefab;
        public Vector3 position;
    }

    [Serializable]
    public struct TranslateParameters {
        public Vector3Int distance;
        public bool blockRects;
        public bool decorationRects;
        public bool interactables;
        public bool lightingFixtures;
        public bool setPieces;
        public bool levelTransitions;
    }

    [Serializable]
    public class CutsceneInfo {
        public GameObject camera;
        public float length;
        public bool fadeMusic;
        public bool suspendInput;
    }
}
