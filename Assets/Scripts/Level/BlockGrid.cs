using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/BlockGrid")]
public class BlockGrid : ScriptableObject {

    public enum StandardBlockType {
        Empty,
        Surface,
        Floor,
        Cube,
        WallNorth,
        WallEast,
        WallSouth,
        WallWest,
        RampNorth,
        RampEast,
        RampSouth,
        RampWest,
        RampNorthEast,
        RampSouthEast,
        RampSouthWest,
        RampNorthWest,
        LadderNorth,
        LadderEast,
        LadderSouth,
        LadderWest,
        DoorNorth,
        DoorEast,
        DoorSouth,
        DoorWest,
        Decoration,
        WallCornerNortheast,
        WallCornerSoutheast,
        WallCornerSouthwest,
        WallCornerNorthwest,
        Interactable
    }

    [System.Serializable]
    public class BlockRect {
        public StandardBlockType blockType;
        public int prefabSetNumber;
        public Vector3 corner0;
        public Vector3 corner1;
    }

    [System.Serializable]
    public class DecorationRect {
        public int decorationIndex;
        public int decorationSetNumber;
        public Vector3 corner0;
        public Vector3 corner1;
    }

    [System.Serializable]
    public class SetPieceInstance {
        public SetPiece setPiece;
        public Vector3Int cornerLocation;
    }

    [System.Serializable]
    public class InteractableInfo {
        public GameObject objectPrefab;
        public Vector3Int position;
        public int stateIndex;
        public Vector3Int[] targetPositions;
    }

    public int xSize;
    public int ySize;
    public int zSize;

    public BlockRect[] blockRects;
    public DecorationRect[] decorationRects;
    public SetPieceInstance[] setPieces;
    public InteractableInfo[] interactables;
}
