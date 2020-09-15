using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLight : Empty, TriggerableObject {

    [HideInInspector]
    public bool state;

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        state = stateIndex > 0;
        gameObject.SetActive(state);
    }

    public void Trigger() {
        state = !state;
        gameObject.SetActive(state);
    }
}
