using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InteractableObject {
    void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation);
}
