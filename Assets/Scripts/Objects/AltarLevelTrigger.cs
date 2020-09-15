using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarLevelTrigger : AltarTrigger {

    public string boolFlagPrefix;

    private string fullFlagName;

    public override void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        fullFlagName = boolFlagPrefix + stateIndex.ToString();
        state = SaveDataManager.instance.GetBoolFlag(fullFlagName);

        GetComponent<Animator>().SetBool("Active", state);
        Color color = state ? onColor : offColor;
        sphere.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);

        lightMaxIntensity = sphere.GetComponent<Light>().intensity;
        sphere.GetComponent<Light>().intensity = state ? lightMaxIntensity : 0;

        playSound = false;

        targets = new TriggerableObject[targetPositions.Length];
        for (int i = 0; i < targetPositions.Length; i++) {
            targets[i] = LevelRenderer.instance.GetObject(targetPositions[i] + cornerLocation).GetComponent<TriggerableObject>();
        }
    }

    public override void Trigger() {
        if (!state) {
            triggered = true;
            state = true;
            playSound = true;

            GetComponent<Animator>().SetTrigger("Activate");

            LevelRenderer.instance.levelObject.PlayCutsceneIndex(0);
            SaveDataManager.instance.SetBoolFlag(fullFlagName, true);
        }
    }
}
