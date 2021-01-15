using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbRingTrigger : Block, InteractableObject {

    [HideInInspector]
    public TriggerableObject[] targets;

    public RingFacingDirection facingDirection;

    public bool useTriggerInput;
    public int triggerInput;

    [HideInInspector]
    public bool triggered = false;

    public enum RingFacingDirection {
        NorthSouth,
        EastWest
    }

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        facingDirection = (RingFacingDirection)stateIndex;

        targets = new TriggerableObject[targetPositions.Length];
        for (int i = 0; i < targetPositions.Length; i++) {
            targets[i] = LevelRenderer.instance.GetBlock(targetPositions[i] + cornerLocation).GetComponent<TriggerableObject>();
        }

        if (facingDirection == RingFacingDirection.NorthSouth) {
            transform.Rotate(new Vector3(0, 0, 90));
        }
    }

    public void Trigger() {
        if (triggered) return;
        if (useTriggerInput) {
            foreach (TriggerableObject obj in targets) {
                if (obj is InputTriggerObject) {
                    InputTriggerObject inputTriggerObject = (InputTriggerObject)obj;
                    inputTriggerObject.Trigger(triggerInput);
                } else {
                    obj.Trigger();
                }
            }
        } else {
            foreach (TriggerableObject obj in targets) {
                obj.Trigger();
            }
        }

        if (GetComponent<AudioSource>()) {
            GetComponent<AudioSource>().Play();
        }

        foreach (Light light in GetComponentsInChildren<Light>()) {
            light.enabled = true;
        }

        triggered = true;
    }

    public void ResetTrigger() {
        triggered = false;
        foreach (Light light in GetComponentsInChildren<Light>()) {
            light.enabled = false;
        }
    }
}
