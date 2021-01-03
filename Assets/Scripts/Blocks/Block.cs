using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VR;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class Block : MonoBehaviour {

    public PathRules defaultPathRules;
    public PathRules[] alternatePathRules;

    [HideInInspector]
    public PathRules pathfindingRules;

    [HideInInspector]
    public Block containedBlock;

    public enum Direction {
        DownSouthWest,
        DownSouth,
        DownSouthEast,
        SouthWest,
        South,
        SouthEast,
        UpSouthWest,
        UpSouth,
        UpSouthEast,
        DownWest,
        Down,
        DownEast,
        West,
        None,
        East,
        UpWest,
        Up,
        UpEast,
        DownNorthWest,
        DownNorth,
        DownNorthEast,
        NorthWest,
        North,
        NorthEast,
        UpNorthWest,
        UpNorth,
        UpNorthEast
    }

    public static Direction Opposite(Direction d) {
        return 26 - d;
    }

    // Central location where the block is spawned
    public Vector3Int gridLocation;

    // Bottom-south-western corner grid position
    public Vector3Int bottomCorner;

    // Top-north-east corner grid position
    public Vector3Int topCorner = Vector3Int.one;

    public virtual bool surfaceAbove {
        get { return false; }
    }

    public virtual void Setup() {
        pathfindingRules = defaultPathRules;
    }

    public virtual void ChangePathRules(int index) {
        pathfindingRules = alternatePathRules[index];
        LevelRenderer.instance.levelObject.pathfindingGraph.UpdateNode(gridLocation);
    }

    public virtual bool MovePlayerHere() {
        return false;
    }

    public virtual bool FindPathHere() {
        return false;
    } 

}

