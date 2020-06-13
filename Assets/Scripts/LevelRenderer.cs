using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelRenderer : MonoBehaviour {

    public static LevelRenderer instance;

    public PathfindingRules pathfindingRules;

    public BlockGrid blockGrid;

    public LevelPrefabSet levelPrefabSet;

    [HideInInspector]
    public BlockGrid.BlockType[] blocks;

    public PathfindingGraph pathfindingGraph;

    //[HideInInspector]
    public GameObject[] blockObjects;

    public AStarPathfinding pathfinder;

    public Player player;

    private void Awake() {
        instance = this;

        blockObjects = new GameObject[blockGrid.xSize * blockGrid.ySize * blockGrid.zSize];

        blocks = (BlockGrid.BlockType[])blockGrid.blocks.Clone();
        LoadRects();

        for (int x = 0; x < blockGrid.xSize; x++) {
            for (int y = 0; y < blockGrid.ySize; y++) {
                for (int z = 0; z < blockGrid.zSize; z++) {
                    BlockGrid.BlockType block = blocks[x + y * blockGrid.xSize + z * blockGrid.xSize * blockGrid.ySize];
                    Vector3 position = new Vector3(x, y, z);
                    GameObject prefab = levelPrefabSet.blockPrefabs[(int)blocks[x + y * blockGrid.xSize + z * blockGrid.xSize * blockGrid.ySize]];
                    if (prefab == null) continue;

                    GameObject obj = Instantiate(prefab, prefab.transform.position + position, prefab.transform.rotation);
                    obj.transform.parent = transform;
                    obj.GetComponent<BlockInfo>().gridLocation = new Vector3(x, y, z);
                    blockObjects[x + y * blockGrid.xSize + z * blockGrid.xSize * blockGrid.ySize] = obj;
                }
            }
        }

        pathfindingGraph = new PathfindingGraph();
        pathfindingGraph.InitializeGraph();

        pathfinder = new AStarPathfinding(pathfindingGraph);
    }

    public void LoadRects() {
        for (int i = 0; i < blockGrid.blockRects.Length; i++) {
            int xMin = (int)Math.Min(blockGrid.blockRects[i].corner0.x, blockGrid.blockRects[i].corner1.x);
            int xMax = (int)Math.Max(blockGrid.blockRects[i].corner0.x, blockGrid.blockRects[i].corner1.x);
            int yMin = (int)Math.Min(blockGrid.blockRects[i].corner0.y, blockGrid.blockRects[i].corner1.y);
            int yMax = (int)Math.Max(blockGrid.blockRects[i].corner0.y, blockGrid.blockRects[i].corner1.y);
            int zMin = (int)Math.Min(blockGrid.blockRects[i].corner0.z, blockGrid.blockRects[i].corner1.z);
            int zMax = (int)Math.Max(blockGrid.blockRects[i].corner0.z, blockGrid.blockRects[i].corner1.z);

            for (int x = xMin; x < xMax; x++) {
                for (int y = yMin; y < yMax; y++) {
                    for (int z = zMin; z < zMax; z++) {
                        blocks[x + y * blockGrid.xSize + z * blockGrid.xSize * blockGrid.ySize] = blockGrid.blockRects[i].blockType;
                    }
                }
            }
        }
    }
}
