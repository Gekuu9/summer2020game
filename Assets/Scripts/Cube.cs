using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : BlockHandler {
    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            FindPathHere();
        }
    }

    public override void MovePlayerHere() {
        Vector3 postion = GetComponent<BlockInfo>().gridLocation;
        int surfaceIndex = (int)(postion.x + postion.y * LevelRenderer.instance.blockGrid.xSize + postion.z * LevelRenderer.instance.blockGrid.xSize * LevelRenderer.instance.blockGrid.ySize) + LevelRenderer.instance.blockGrid.xSize;
        LevelRenderer.instance.blockObjects[surfaceIndex].GetComponent<Surface>().MovePlayerHere();
    }

    public override void FindPathHere() {
        Vector3 postion = GetComponent<BlockInfo>().gridLocation;
        int surfaceIndex = (int)(postion.x + postion.y * LevelRenderer.instance.blockGrid.xSize + postion.z * LevelRenderer.instance.blockGrid.xSize * LevelRenderer.instance.blockGrid.ySize) + LevelRenderer.instance.blockGrid.xSize;
        LevelRenderer.instance.blockObjects[surfaceIndex].GetComponent<Surface>().FindPathHere();
    }
}
