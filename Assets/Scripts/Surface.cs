using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : BlockHandler {
    public override void FindPathHere() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        LevelRenderer.instance.player.PathMove(location);
    }

    public override void MovePlayerHere() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        LevelRenderer.instance.player.Move(location, 3f);
    }
}
