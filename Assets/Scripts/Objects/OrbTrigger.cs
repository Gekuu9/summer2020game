using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbTrigger : Block, InteractableObject {

    [HideInInspector]
    public TriggerableObject[] targets;

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        targets = new TriggerableObject[targetPositions.Length];
        for (int i = 0; i < targetPositions.Length; i++) {
            targets[i] = LevelRenderer.instance.GetBlock(targetPositions[i] + cornerLocation).GetComponent<TriggerableObject>();
        }
    }

    public void Trigger() {
        foreach (TriggerableObject obj in targets) {
            obj.Trigger();
        }
        if (GetComponent<AudioSource>()) {
            GetComponent<AudioSource>().Play();
        }
    }
}
