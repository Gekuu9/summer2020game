using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Block {

    public Vector3 movePlayerOffset;

    [HideInInspector]
    public int nextLevelIndex;

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            FindPathHere();
        }
    }

    public override bool FindPathHere() {
        return LevelRenderer.instance.player.PathMove(gridLocation);
    }

    public override bool MovePlayerHere() {
        LevelRenderer.instance.player.Move(gridLocation + movePlayerOffset, 3f, false);
        LevelRenderer.instance.LevelTransition(nextLevelIndex);
        return true;
    }
}
