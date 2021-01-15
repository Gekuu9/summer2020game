using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ToggleTorch : Block, TriggerableObject {

    public string[] requiredFlagNames;
    public bool allFlagsRequired;
    public bool oneTimeTurnOn;

    [HideInInspector]
    public bool isTorchOn;

    public virtual void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        isTorchOn = stateIndex > 0;

        foreach (string flagName in requiredFlagNames) {
            if (SaveDataManager.instance.GetBoolFlag(flagName)) {
                isTorchOn = true;
                if (!allFlagsRequired) {
                    break;
                }
            } else {
                isTorchOn = false;
                if (allFlagsRequired) {
                    break;
                }
            }
        }

        transform.Find("Fire").gameObject.SetActive(isTorchOn);
        foreach (AudioSource source in GetComponentsInChildren<AudioSource>()) {
            if (!isTorchOn) source.Stop();
        }
    }

    public void Trigger() {
        if (isTorchOn && oneTimeTurnOn) return;
        isTorchOn = !isTorchOn;
        transform.Find("Fire").gameObject.SetActive(isTorchOn);
        foreach (AudioSource source in GetComponentsInChildren<AudioSource>()) {
            if (isTorchOn) source.Play();
            else source.Stop();
        }
    }

    public override bool FindPathHere() {
        Vector3 location = gridLocation;
        return LevelRenderer.instance.player.PathMove(location, true);
    }

    public override bool MovePlayerHere() {
        Vector3 location = gridLocation;
        LevelRenderer.instance.player.Move(location, 3f);
        return true;
    }
}
