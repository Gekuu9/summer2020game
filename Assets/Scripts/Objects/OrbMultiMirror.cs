using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbMultiMirror : OrbMirror {

    public List<List<FaceDirection>> possibleDirections;

    [HideInInspector]
    public List<FaceDirection> currentDirections;

    public override void UpdateFaceDirection(int newDirection) {
        currentDirections = possibleDirections[newDirection];
    }
}
