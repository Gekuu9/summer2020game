using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Policy;
using Unity.Collections;
using UnityEngine;

public class VolumeTrigger : Empty, InteractableObject {

    public bool oneTimeTrigger;

    public TriggerableObject[] targets;

    private bool triggered;

    public void Trigger() {
        for (int i = 0; i < targets.Length; i++) {
            targets[i].Trigger();
        }
        triggered = true;
    }

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        targets = new TriggerableObject[targetPositions.Length];
        for (int i = 0; i < targetPositions.Length; i++) {
            targets[i] = LevelRenderer.instance.GetBlock(targetPositions[i] + cornerLocation).GetComponent<TriggerableObject>();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>()) {
            if (!oneTimeTrigger || !triggered) {
                Trigger();
            }
        }
    }
}
