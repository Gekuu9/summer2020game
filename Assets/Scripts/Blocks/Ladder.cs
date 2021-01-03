using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Block {

    public Vector3 faceDirection;

    public override bool surfaceAbove {
        get { return true; }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            FindPathHere();
        }
    }

    public override bool FindPathHere() {
        return LevelRenderer.instance.player.PathMove(gridLocation);
    }

    public override bool MovePlayerHere() {
        LevelRenderer.instance.player.Move(gridLocation, 2f, false);
        if (LevelRenderer.instance.player.gridPosition.y < gridLocation.y) LevelRenderer.instance.player.TurnPlayer(faceDirection);
        return true;
    }
}
