using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BlockGrid : ScriptableObject {
    public int xSize;
    public int ySize;
    public int zSize;

    public Block[] blocks;

    public BlockGrid(int x, int y, int z) {
        xSize = x;
        ySize = y;
        zSize = z;

        blocks = new Block[x * y * z];
    }

    public Block getBlock(int x, int y, int z) {
        return blocks[x + y * xSize + z * xSize * ySize];
    }
    public void setBlock(Block block, int x, int y, int z) {
        blocks[x + y * xSize + z * xSize * ySize] = block ;
    }
}
