using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : BlockHandler {

    public Vector3 faceDirection;

    public override bool surfaceAbove {
        get { return true; }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            FindPathHere();
        }
    }

    private void OnMouseEnter() {
        GetComponent<OutlineOrthoSingle>().enabled = true;
    }

    private void OnMouseExit() {
        GetComponent<OutlineOrthoSingle>().enabled = false;
    }

    private void OnEnable() {
        GetComponent<OutlineOrthoSingle>().enabled = false;
    }

    public override bool FindPathHere() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        return LevelRenderer.instance.player.PathMove(location);
    }

    public override void MovePlayerHere() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        LevelRenderer.instance.player.Move(location, 2f, false);
        if (LevelRenderer.instance.player.gridPosition.y < location.y) LevelRenderer.instance.player.TurnPlayer(faceDirection);
    }
}
