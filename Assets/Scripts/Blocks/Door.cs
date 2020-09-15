using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : BlockHandler {

    public Vector3 movePlayerOffset;

    [HideInInspector]
    public int nextLevelIndex;

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            FindPathHere();
        }
    }

    private void OnMouseEnter() {
        if (GetComponent<OutlineOrthoSingle>()) GetComponent<OutlineOrthoSingle>().enabled = true;
        if (GetComponent<OutlineOrtho>()) GetComponent<OutlineOrtho>().enabled = true;
        if (transform.Find("Arrow")) {
            transform.Find("Arrow").gameObject.SetActive(true);
        }
    }

    private void OnMouseExit() {
        if (GetComponent<OutlineOrthoSingle>()) GetComponent<OutlineOrthoSingle>().enabled = false;
        if (GetComponent<OutlineOrtho>()) GetComponent<OutlineOrtho>().enabled = false;
        if (transform.Find("Arrow")) {
            transform.Find("Arrow").gameObject.SetActive(false);
        }
    }

    private void OnEnable() {
        if (GetComponent<OutlineOrthoSingle>()) GetComponent<OutlineOrthoSingle>().enabled = false;
        if (GetComponent<OutlineOrtho>()) GetComponent<OutlineOrtho>().enabled = false;
        if (transform.Find("Arrow")) {
            transform.Find("Arrow").gameObject.SetActive(false);
        }
    }

    public override bool FindPathHere() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        return LevelRenderer.instance.player.PathMove(location);
    }

    public override void MovePlayerHere() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        LevelRenderer.instance.player.Move(location + movePlayerOffset, 3f);
        LevelRenderer.instance.LevelTransition(nextLevelIndex);
    }
}
