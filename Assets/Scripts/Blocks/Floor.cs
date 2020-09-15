using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : BlockHandler {

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

    public override void MovePlayerHere() {
        Vector3Int position = Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation);
        position.y += 1;
        LevelRenderer.instance.GetObject(position).GetComponent<BlockHandler>().MovePlayerHere();
    }

    public override bool FindPathHere() {
        Vector3Int position = Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation);
        position.y += 1;
        return LevelRenderer.instance.GetObject(position).GetComponent<BlockHandler>().FindPathHere();
    }

    private void OnEnable() {
        GetComponent<OutlineOrthoSingle>().enabled = false;
    }
}
