using System.Collections;
using System.Collections.Generic;
using Cinemachine.Editor;
using UnityEngine;

public class ToggleBlock : Block, TriggerableObject {

    public bool state;

    public GameObject movingBlock;

    public string toggleOnName;
    public string toggleOffName;

    public Quaternion onRotation;
    public Quaternion offRotation;

    public Vector3 onOffset;
    public Vector3 offOffset;

    public Vector3 onScale;
    public Vector3 offScale;

    public override bool surfaceAbove {
        get { return state; }
    }

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        state = stateIndex > 0;

        if (state) {
            movingBlock.transform.localPosition += onOffset;
            movingBlock.transform.localRotation = onRotation;
            movingBlock.transform.localScale = onScale;
        }
        else {
            movingBlock.transform.localPosition += offOffset;
            movingBlock.transform.localRotation = offRotation;
            movingBlock.transform.localScale = offScale;
        }

        UpdatePathfinding();
    }

    public void UpdatePathfinding() {
        ChangePathRules(state ? 1 : 0);
        Vector3Int location = Vector3Int.RoundToInt(gridLocation);
        if (location.y + 1 < LevelRenderer.instance.levelObject.ySize) {
            location.y += 1;
            Block aboveBlock = LevelRenderer.instance.GetBlock(location);
            if (aboveBlock.GetComponent<Empty>()) {
                aboveBlock.GetComponent<Empty>().UpdatePathfinding();
                LevelRenderer.instance.levelObject.pathfindingGraph.UpdateNode(location);
            }
        }
        GetComponent<BoxCollider>().enabled = state;
        foreach (BoxCollider c in GetComponentsInChildren<BoxCollider>()) c.enabled = state;
    }

    public void PlayAnimation() {
        if (state) {
            iTween.MoveTo(movingBlock, gridLocation + onOffset, 2f);
            iTween.ScaleTo(movingBlock, onScale, 2f);
        }
        else {
            iTween.MoveTo(movingBlock, gridLocation + offOffset, 2f);
            iTween.ScaleTo(movingBlock, offScale, 2f);
        }
    }

    public void Trigger() {
        state = !state;
        PlayAnimation();
        UpdatePathfinding();
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            FindPathHere();
        }
    }

    public override bool MovePlayerHere() {
        if (!state) {
            Vector3 location = gridLocation;
            LevelRenderer.instance.player.Move(location, 3f);
            return true;
        } else {
            return false;
        }
    }

    public override bool FindPathHere() {
        if (!state) {
            Vector3 location = gridLocation;
            return LevelRenderer.instance.player.PathMove(location);
        } else {
            Vector3Int position = Vector3Int.RoundToInt(gridLocation);
            position.y += 1;
            return LevelRenderer.instance.GetBlock(position).FindPathHere();
        }
    }
}
