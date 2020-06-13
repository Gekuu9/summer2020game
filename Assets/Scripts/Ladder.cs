using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : BlockHandler {

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            FindPathHere();
        }
    }

    public override void FindPathHere() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        LevelRenderer.instance.player.PathMove(location);
    }

    public override void MovePlayerHere() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        LevelRenderer.instance.player.Move(location, 0.5f);
    }
}
