using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Policy;
using Unity.Collections;
using UnityEngine;

public class VolumeTrigger : Empty, InteractableObject {

    public bool oneTimeTrigger;

    public TriggerableObject[] targets;

    public Vector3 corner0;
    public Vector3 corner1;

    private bool triggered;

    void Update() {
        if (triggered && oneTimeTrigger) return;
        Vector3 playerPosition = LevelRenderer.instance.player.transform.position;
        Vector3 corner0Location = corner0 + GetComponent<BlockInfo>().gridLocation;
        Vector3 corner1Location = corner1 + GetComponent<BlockInfo>().gridLocation;
        if (playerPosition.x >= corner0Location.x && playerPosition.x < corner1Location.x && playerPosition.y >= corner0Location.y && playerPosition.y < corner1Location.y && playerPosition.z >= corner0Location.z && playerPosition.z < corner1Location.z) {
            if (!triggered) Trigger();
        } else {
            if (triggered) triggered = false;
        }
    }

    public void Trigger() {
        for (int i = 0; i < targets.Length; i++) {
            targets[i].Trigger();
        }
        triggered = true;
    }

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        targets = new TriggerableObject[targetPositions.Length];
        for (int i = 0; i < targetPositions.Length; i++) {
            targets[i] = LevelRenderer.instance.GetObject(targetPositions[i] + cornerLocation).GetComponent<TriggerableObject>();
        }
    }
}
