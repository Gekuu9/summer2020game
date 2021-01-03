using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : Block {

    public float yOffset;

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            FindPathHere();
        }
    }

    public override bool FindPathHere() {
        return LevelRenderer.instance.player.PathMove(gridLocation);
    }

    public override bool MovePlayerHere() {
        Vector3 location = gridLocation;
        location.y += yOffset;
        LevelRenderer.instance.player.Move(location, 3f);
        return true;
        
    }
}
