using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : BlockHandler {
    [HideInInspector]
    public bool isSurface;
    [HideInInspector]
    public bool forceSurface = false;

    private void Start() {
        UpdatePathfinding();
    }

    public void UpdatePathfinding() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        if (location.y <= 0 && !forceSurface) return;
        location.y -= 1;
        GameObject obj = LevelRenderer.instance.GetObject(Vector3Int.RoundToInt(location));
        if ((obj != null && obj.GetComponent<BlockHandler>() != null && obj.GetComponent<BlockHandler>().surfaceAbove) || forceSurface) {
            GetComponent<PFRulesVariable>().SwitchRuleset(1);
            isSurface = true;
        }
        else {
            GetComponent<PFRulesVariable>().SwitchRuleset(0);
            isSurface = false;
        }
    }

    public override bool FindPathHere() {
        if (!isSurface) return false;
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        return LevelRenderer.instance.player.PathMove(location);
    }

    public override void MovePlayerHere() {
        if (!isSurface) return;
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        LevelRenderer.instance.player.Move(location, 3f);
    }
}
