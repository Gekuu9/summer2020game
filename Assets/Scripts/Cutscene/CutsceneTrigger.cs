using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : Empty, TriggerableObject {

    [HideInInspector]
    public int cutsceneIndex;

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerlocation) {
        cutsceneIndex = stateIndex;
    }

    public void Trigger() {
        LevelRenderer.instance.levelObject.PlayCutsceneIndex(cutsceneIndex);
    }
}
