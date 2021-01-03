using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Block, InteractableObject {

    public string upAnimationName;
    public string downAnimationName;

    public Transform pivot;

    [HideInInspector]
    public TriggerableObject[] targets;

    [HideInInspector]
    public bool state;

    public Vector3 upOffset;
    public Vector3 downOffset;

    public Vector3 upScale;
    public Vector3 downScale;

    public int numColliders = 0;

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        state = stateIndex > 0;
        targets = new TriggerableObject[targetPositions.Length];
        for (int i = 0; i < targetPositions.Length; i++) {
            targets[i] = LevelRenderer.instance.GetBlock(targetPositions[i] + cornerLocation).GetComponent<TriggerableObject>();
        }

        if (state) {
            pivot.position += downOffset;
            pivot.localScale = downScale;
        }
        else {
            pivot.position += upOffset;
            pivot.localScale = upScale;
        }
    }

    public void Toggle() {
        state = !state;
        PlayAnimation();
        foreach (TriggerableObject target in targets) {
            target.Trigger();
        }
    }

    public void PlayAnimation() {
        if (state) {
            GetComponent<Animation>().Play(downAnimationName);
        }
        else {
            GetComponent<Animation>().Play(upAnimationName);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<Player>() || other.gameObject.GetComponent<PushBlock>()) {
            if (numColliders == 0) Toggle();
            numColliders++;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponent<Player>() || other.gameObject.GetComponent<PushBlock>()) {
            if (numColliders == 1) Toggle();
            numColliders--;
        }
    }

    private void OnMouseEnter() {
        Vector3Int position = Vector3Int.RoundToInt(gridLocation);
        position.y -= 1;
        LevelRenderer.instance.GetBlock(position).GetComponent<OutlineOrthoSingle>().enabled = true;
    }

    private void OnMouseExit() {
        Vector3Int position = Vector3Int.RoundToInt(gridLocation);
        position.y -= 1;
        LevelRenderer.instance.GetBlock(position).GetComponent<OutlineOrthoSingle>().enabled = false;
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            FindPathHere();
        }
    }

    public override bool FindPathHere() {
        return LevelRenderer.instance.player.PathMove(gridLocation);
    }

    public override bool MovePlayerHere() {
        LevelRenderer.instance.player.Move(gridLocation, 3f);
        return true;
    }
}
