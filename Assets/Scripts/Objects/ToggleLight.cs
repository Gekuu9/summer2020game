using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLight : Empty, TriggerableObject {

    public bool fadeInOut;
    public float fadeSpeed;

    public string[] requiredFlagNames;
    public bool allFlagsRequired;

    [HideInInspector]
    public bool isLightOn;

    private float initialIntensity;

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        isLightOn = stateIndex > 0;

        foreach (string flagName in requiredFlagNames) {
            if (SaveDataManager.instance.GetBoolFlag(flagName)) {
                isLightOn = true;
                if (!allFlagsRequired) {
                    break;
                }
            }
            else {
                isLightOn = false;
                if (allFlagsRequired) {
                    break;
                }
            }
        }

        initialIntensity = GetComponent<Light>().intensity;
        GetComponent<Light>().intensity = isLightOn ? initialIntensity : 0;
    }

    public void Trigger() {
        isLightOn = !isLightOn;
        if (!fadeInOut) {
            GetComponent<Light>().intensity = isLightOn ? initialIntensity : 0;
        }
    }

    private void Update() {
        if (fadeInOut) {
            Light light = GetComponent<Light>();
            if (isLightOn && light.intensity < initialIntensity) {
                light.intensity += fadeSpeed * Time.deltaTime;
                if (light.intensity > initialIntensity) light.intensity = initialIntensity;
            } else if (!isLightOn && light.intensity > 0) {
                light.intensity -= fadeSpeed * Time.deltaTime;
                if (light.intensity < 0) light.intensity = 0;
            }
        }
    }
}
