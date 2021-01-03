﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitch : Block, InteractableObject {

    public TriggerableObject[] targets;
    public bool state;

    public Transform pivot;

    public string toggleOnName;
    public string toggleOffName;

    public Quaternion onRotation;
    public Quaternion offRotation;

    public Vector3 onOffset;
    public Vector3 offOffset;

    public Vector3 onScale;
    public Vector3 offScale;

    private bool waitingForPlayer;

    public void PlayAnimation() {
        if (state) {
            GetComponent<Animation>().Play(toggleOnName);
        }
        else {
            GetComponent<Animation>().Play(toggleOffName);
        }
    }

    private void Update() {
        if (waitingForPlayer) {
            if (LevelRenderer.instance.player.pathTargetPosition != gridLocation) {
                waitingForPlayer = false;
            } else if (LevelRenderer.instance.player.gridPosition == gridLocation) {
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

        if (state) {
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
        state = !state;
        PlayAnimation();
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
        return LevelRenderer.instance.player.PathMove(location);
    }
}
