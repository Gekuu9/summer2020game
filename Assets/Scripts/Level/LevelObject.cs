using System;
using System.Collections;
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

    // The graph used to store information about where the player can move
    public PathfindingGraph pathfindingGraph;
    // The pathfinder that gives the shortest path the player can take to a location
    public AStarPathfinding pathfinder;

    // The block type at each location on the grid
    [HideInInspector]
    public BlockGrid.BlockType[] blocks;
    // The decoration index used to determine which decoration to place at each location on the grid
    [HideInInspector]
    public int[] decorationIndices;
    // The index of the block set that the prefabs at each location on the grid are stored in
    [HideInInspector]
    public int[] blockPrefabIndices;
    // The gameobjects for each of the blocks on the grid
    [HideInInspector]
    public GameObject[] blockObjects;
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

        blockObjects = new GameObject[xSize * ySize * zSize];
        blocks = new BlockGrid.BlockType[xSize * ySize * zSize];
        blockPrefabIndices = new int[xSize * ySize * zSize];
        decorationIndices = new int[xSize * ySize * zSize];

        // Load block info from BlockGrid
        LoadBlockRects(blockGrid.blockRects, Vector3Int.zero);
        LoadDecorationRects(blockGrid.decorationRects, Vector3Int.zero);
        LoadInteractables(blockGrid.interactables, Vector3Int.zero);

        // Load block info from set pieces
        foreach (BlockGrid.SetPieceInstance setPiece in blockGrid.setPieces) {
            LoadBlockRects(setPiece.setPiece.blockRects, setPiece.cornerLocation);
            LoadDecorationRects(setPiece.setPiece.decorationRects, setPiece.cornerLocation);
            LoadInteractables(setPiece.setPiece.interactables, setPiece.cornerLocation);
        }

        // Use loaded block info to instantiate block objects
        LoadBlocks();

        // Load transition info into door objects
        LoadTransitions();

        // Instantiate lighting objects
        LoadLighting();
    }

    public void LoadBlocks() {
        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++) {
                for (int z = 0; z < zSize; z++) {
                    int index = x + y * xSize + z * xSize * ySize;
                    // BlockGrid.BlockType block = blocks[index];
                    Vector3 position = new Vector3(x, y, z);
                    int prefabIndex = blockPrefabIndices[index];
                    GameObject prefab;
                    switch (blocks[index]) {
                        case BlockGrid.BlockType.Decoration:
                            prefab = levelDecorationSets[prefabIndex].decorationPrefabs[decorationIndices[index]];
                            break;
                        case BlockGrid.BlockType.Interactable:
                            prefab = blockObjects[index];
                            break;
                        case BlockGrid.BlockType.Empty:
                            if (y == 0) {
                                continue;
                            } else if (blocks[index - xSize] == BlockGrid.BlockType.Empty) {
                                continue;
                            } else {
                                prefab = levelBlockSets[prefabIndex].blockPrefabs[(int)blocks[index]];
                            }
                            break;
                        default:
                            prefab = levelBlockSets[prefabIndex].blockPrefabs[(int)blocks[index]];
                            break;
                    }

                    if (prefab == null) continue;
                    if (gameObject == null) return;

                    GameObject obj = Instantiate(prefab, prefab.transform.position + position, prefab.transform.rotation);
                    obj.transform.parent = transform;
                    obj.GetComponent<BlockInfo>().gridLocation = new Vector3(x, y, z);
                    blockObjects[index] = obj;
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
                        blocks[index] = blockRects[i].blockType;
                        blockPrefabIndices[index] = blockRects[i].prefabSetNumber;
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
                        blocks[index] = BlockGrid.BlockType.Decoration;
                        decorationIndices[index] = decorationRects[i].decorationIndex;
                        blockPrefabIndices[index] = decorationRects[i].decorationSetNumber;
                    }
                }
            }
        }
    }

    public void LoadInteractables(BlockGrid.InteractableInfo[] interactables, Vector3Int cornerLocation) {
        foreach (BlockGrid.InteractableInfo info in interactables) {
            Vector3Int position = info.position + cornerLocation;
            int index = position.x + position.y * xSize + position.z * xSize * ySize;
            blocks[index] = BlockGrid.BlockType.Interactable;
            blockObjects[index] = info.objectPrefab;
        }
    }

    public void LoadTransitions() {
        for (int i = 0; i < levelTransitionInfo.Length; i++) {
            LevelTransitionInfo t = levelTransitionInfo[i];
            Door door = GetObject(Vector3Int.RoundToInt(t.doorLocation)).GetComponent<Door>();
            if (door == null) continue;
            door.nextLevelIndex = i;
        }
    }

    public void LoadLighting() {
        foreach (LightingFixture light in lightingFixtures) {
            GameObject l = Instantiate(light.lightingPrefab, transform);
            l.transform.position = light.position;
        }
    }

    public void SetupInteractables() {
        foreach (BlockGrid.InteractableInfo info in blockGrid.interactables) {
            GameObject obj = GetObject(info.position);
            obj.GetComponent<InteractableObject>().Setup(info.stateIndex, info.targetPositions, Vector3Int.zero);
        }
        foreach (BlockGrid.SetPieceInstance setPiece in blockGrid.setPieces) {
            foreach (BlockGrid.InteractableInfo info in setPiece.setPiece.interactables) {
                GameObject obj = GetObject(info.position + setPiece.cornerLocation);
                obj.GetComponent<InteractableObject>().Setup(info.stateIndex, info.targetPositions, setPiece.cornerLocation);
            }
        }
    }

    public GameObject GetObject(Vector3Int location) {
        return blockObjects[location.x + location.y * xSize + location.z * xSize * ySize];
    }

    public void SetObject(Vector3Int location, GameObject obj) {
        blockObjects[location.x + location.y * xSize + location.z * xSize * ySize] = obj;
    }

    public void SwapObjects(Vector3Int location1, Vector3Int location2) {
        GameObject temp = blockObjects[location1.x + location1.y * xSize + location1.z * xSize * ySize];
        blockObjects[location1.x + location1.y * xSize + location1.z * xSize * ySize] = blockObjects[location2.x + location2.y * xSize + location2.z * xSize * ySize];
        blockObjects[location2.x + location2.y * xSize + location2.z * xSize * ySize] = temp;
    }

    public void CreateBlockRect(Vector3 corner0, Vector3 corner1, BlockGrid.BlockType type, int prefabSetNumber = 0) {
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
                    if (type == BlockGrid.BlockType.Surface) blocks[index] = BlockGrid.BlockType.Empty;
                    else blocks[index] = type;
                    blockPrefabIndices[index] = prefabSetNumber;

                    
                    GameObject prefab = levelBlockSets[prefabSetNumber].blockPrefabs[(int)blocks[index]];
                    if (prefab == null) continue;
                    if (gameObject == null) return;

                    GameObject obj = Instantiate(prefab, prefab.transform.position + position, prefab.transform.rotation);
                    obj.transform.parent = transform;
                    obj.GetComponent<BlockInfo>().gridLocation = new Vector3(x, y, z);

                    if (type == BlockGrid.BlockType.Empty) obj.GetComponent<Empty>().UpdatePathfinding();
                    if (type == BlockGrid.BlockType.Surface) {
                        obj.GetComponent<Empty>().forceSurface = true;
                        obj.GetComponent<Empty>().UpdatePathfinding();
                    }

                    if (blockObjects[index] != null) {
                        blockObjects[index] = obj;
                        pathfindingGraph.UpdateNode(Vector3Int.RoundToInt(position), obj);
                    } else {
                        blockObjects[index] = obj;
                        pathfindingGraph.CreateNode(Vector3Int.RoundToInt(position));
                    }
                }
            }
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
