using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ToggleTorch : BlockHandler, TriggerableObject {

    [HideInInspector]
    public bool state;

    public virtual void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        state = stateIndex > 0;
        GetComponentInChildren<Light>().enabled = state;
        if (state) {
            GetComponentInChildren<ParticleSystem>().Play();
        } else {
            GetComponentInChildren<ParticleSystem>().Stop();
        }
    }

    public void Trigger() {
        state = !state;
        GetComponentInChildren<Light>().enabled = state;
        if (state) {
            GetComponentInChildren<ParticleSystem>().Play();
        } else {
            GetComponentInChildren<ParticleSystem>().Stop();
        }
    }

    public override bool FindPathHere() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        return LevelRenderer.instance.player.PathMove(location, true);
    }

    public override void MovePlayerHere() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        LevelRenderer.instance.player.Move(location, 3f);
    }
}
