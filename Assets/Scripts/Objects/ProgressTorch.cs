using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTorch : ToggleTorch {

    public string flagNamePrefix;

    private int flagIndex;
    private string fullFlagName;

    private bool setup = false;
    
    public override void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        flagIndex = stateIndex;
        fullFlagName = flagNamePrefix + stateIndex.ToString();
        isTorchOn = SaveDataManager.instance.GetBoolFlag(fullFlagName);

        transform.Find("Fire").gameObject.SetActive(isTorchOn);

        setup = true;
    }

    
    private void OnEnable() {
        if (!setup) return;
        bool newState = SaveDataManager.instance.GetBoolFlag(fullFlagName);
        if (!isTorchOn && newState) {
            LevelRenderer.instance.levelObject.PlayCutsceneIndex(flagIndex - 1);
        }
    }
    
}
