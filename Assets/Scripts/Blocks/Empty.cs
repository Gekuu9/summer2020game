using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : Block {
    [HideInInspector]
    public bool isSurface;
    [HideInInspector]
    public bool forceSurface = false;

    private void Start() {
        UpdatePathfinding();
    }

    public void UpdatePathfinding() {
        Vector3Int location = gridLocation;
        if (location.y <= 0 && !forceSurface) return;
        location.y -= 1;
        Block block = LevelRenderer.instance.GetBlock(location);
        if ((block != null && block.surfaceAbove) || forceSurface) {
            if (alternatePathRules.Length > 0) {
                pathfindingRules = alternatePathRules[0];
            } else {
                pathfindingRules = defaultPathRules;
            }
            isSurface = true;
        } else {
            pathfindingRules = defaultPathRules;
            isSurface = false;
        }
    }

    public override bool FindPathHere() {
        if (!isSurface) return false;
        return LevelRenderer.instance.player.PathMove(gridLocation);
    }

    public override bool MovePlayerHere() {
        if (!isSurface) return false;
        LevelRenderer.instance.player.Move(gridLocation, 3f);
        return true;
    }
}
