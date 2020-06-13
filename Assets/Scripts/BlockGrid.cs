using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu]
public class BlockGrid : ScriptableObject {

    public enum BlockType {
        Empty,
        SimpleFloor,
        SimpleWallNorth,
        SimpleWallEast,
        SimpleWallSouth,
        SimpleWallWest,
        Surface,
        SimpleRampNorth,
        SimpleRampEast,
        SimpleRampSouth,
        SimpleRampWest,
        LadderNorth,
        LadderEast,
        LadderSouth,
        LadderWest,
        SimpleCube
    }

    [System.Serializable]
    public class BlockRect {
        public BlockType blockType;
        public Vector3 corner0;
        public Vector3 corner1;
    }

    public int xSize;
    public int ySize;
    public int zSize;

    public BlockRect[] blockRects;

    public BlockType[] blocks;

    /*
    public void LoadRects() {
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
                        blocks[x + y * xSize + z * xSize * ySize] = blockRects[i].blockType;
                    }
                }
            }
        }
    }
    */
}
