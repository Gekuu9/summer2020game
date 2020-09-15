using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTorch : ToggleTorch {

    public string flagNamePrefix;

    private string fullFlagName;

    private bool setup = false;
    
    public override void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        fullFlagName = flagNamePrefix + stateIndex.ToString();
        state = SaveDataManager.instance.GetBoolFlag(fullFlagName);

        transform.Find("Fire").gameObject.SetActive(state);

        setup = true;
    }

    private void OnEnable() {
        if (!setup) return;
        state = SaveDataManager.instance.GetBoolFlag(fullFlagName);

        transform.Find("Fire").gameObject.SetActive(state);
    }
}
