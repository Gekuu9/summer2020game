using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingRules : MonoBehaviour {

    public bool northIn;
    public bool eastIn;
    public bool southIn;
    public bool westIn;
    public bool northEastIn;
    public bool southEastIn;
    public bool southWestIn;
    public bool northWestIn;
    public bool upIn;
    public bool downIn;
    public bool upNorthIn;
    public bool upEastIn;
    public bool upSouthIn;
    public bool upWestIn;
    public bool downNorthIn;
    public bool downEastIn;
    public bool downSouthIn;
    public bool downWestIn;
    public bool upNorthEastIn;
    public bool upSouthEastIn;
    public bool upSouthWestIn;
    public bool upNorthWestIn;
    public bool downNorthEastIn;
    public bool downSouthEastIn;
    public bool downSouthWestIn;
    public bool downNorthWestIn;

    public bool northOut;
    public bool eastOut;
    public bool southOut;
    public bool westOut;
    public bool northEastOut;
    public bool southEastOut;
    public bool southWestOut;
    public bool northWestOut;
    public bool upOut;
    public bool downOut;
    public bool upNorthOut;
    public bool upEastOut;
    public bool upSouthOut;
    public bool upWestOut;
    public bool downNorthOut;
    public bool downEastOut;
    public bool downSouthOut;
    public bool downWestOut;
    public bool upNorthEastOut;
    public bool upSouthEastOut;
    public bool upSouthWestOut;
    public bool upNorthWestOut;
    public bool downNorthEastOut;
    public bool downSouthEastOut;
    public bool downSouthWestOut;
    public bool downNorthWestOut;

    public void UpdatePFGraph() {
        LevelRenderer.instance.levelObject.pathfindingGraph.UpdateNode(Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation));
    }
}
