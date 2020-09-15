using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : BlockHandler {

    public float yOffset;

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

    public override bool FindPathHere() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        return LevelRenderer.instance.player.PathMove(location);
    }

    public override void MovePlayerHere() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        location.y += yOffset;
        LevelRenderer.instance.player.Move(location, 3f);
        
    }

    private IEnumerator WaitForPlayerPosition() {
        while (LevelRenderer.instance.player.moveTimer > 1) yield return null;
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        location.y += yOffset;
        LevelRenderer.instance.player.Move(location, 3f);
    }
}
