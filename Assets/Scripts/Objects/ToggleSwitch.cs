using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitch : Block, InteractableObject {

    public TriggerableObject[] targets;
    public bool state;

    public Transform pivot;

    public bool automaticReset;
    public bool findClosestPath;

    public string toggleOnName;
    public string toggleOffName;
    public string toggleOnOffName;

    public Quaternion onRotation;
    public Quaternion offRotation;

    public Vector3 onOffset;
    public Vector3 offOffset;

    public Vector3 onScale;
    public Vector3 offScale;

    private bool waitingForPlayer = false;
    private bool animationPlaying = false;

    private Vector3 playerTargetPosition;

    public IEnumerator PlayAnimation() {
        animationPlaying = true;
        if (automaticReset) {
            GetComponent<Animation>().Play(toggleOnOffName);
            float length = GetComponent<Animation>().GetClip(toggleOnOffName).length;
            yield return new WaitForSeconds(length);
            animationPlaying = false;
        } else if (state) {
            GetComponent<Animation>().Play(toggleOnName);
            float length = GetComponent<Animation>().GetClip(toggleOnName).length;
            yield return new WaitForSeconds(length);
            animationPlaying = false;
        } else {
            GetComponent<Animation>().Play(toggleOffName);
            float length = GetComponent<Animation>().GetClip(toggleOffName).length;
            yield return new WaitForSeconds(length);
            animationPlaying = false;
        }
    }

    private void Update() {
        if (waitingForPlayer) {
            if (LevelRenderer.instance.player.pathTargetPosition != playerTargetPosition) {
                waitingForPlayer = false;
            } else if (LevelRenderer.instance.player.gridPosition == playerTargetPosition) {
                waitingForPlayer = false;
                Toggle();
            }
        }
    }

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        state = stateIndex > 0;
        targets = new TriggerableObject[targetPositions.Length];
        for (int i = 0; i < targetPositions.Length; i++) {
            targets[i] = LevelRenderer.instance.GetBlock(targetPositions[i] + cornerLocation).GetComponent<TriggerableObject>();
        }
        if (automaticReset) {
            pivot.position += offOffset;
            pivot.rotation = offRotation;
            pivot.localScale = offScale;
        } else if (state) {
            pivot.position += onOffset;
            pivot.rotation = onRotation;
            pivot.localScale = onScale;
        } else {
            pivot.position += offOffset;
            pivot.rotation = offRotation;
            pivot.localScale = offScale;
        }
    }

    public void Toggle() {
        if (animationPlaying) return;
        state = !state;
        IEnumerator coroutine = PlayAnimation();
        StartCoroutine(coroutine);
        foreach (TriggerableObject target in targets) {
            target.Trigger();
        }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            FindPathHere();
            waitingForPlayer = true;
        }
    }

    public override bool MovePlayerHere() {
        Vector3 location = gridLocation;
        LevelRenderer.instance.player.Move(location, 3f);
        return true;
    }

    public override bool FindPathHere() {
        Vector3 location = gridLocation;
        bool success = LevelRenderer.instance.player.PathMove(location, findClosestPath);
        if (success) {
            if (findClosestPath) playerTargetPosition = LevelRenderer.instance.player.pathTargetPosition;
            else playerTargetPosition = gridLocation; 
        }
        return success;
    }
}
