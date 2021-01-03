using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : Block {

    public override bool surfaceAbove {
        get { return true; }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            FindPathHere();
        }
    }

    public override bool MovePlayerHere() {
        Vector3Int position = gridLocation;
        position.y += 1;
        return LevelRenderer.instance.GetBlock(position).MovePlayerHere();
    }

    public override bool FindPathHere() {
        Vector3Int position = gridLocation;
        position.y += 1;
        return LevelRenderer.instance.GetBlock(position).FindPathHere();
    }
}
