using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCreator : Block, TriggerableObject {

    public GameObject orbPrefab;

    public FaceDirection currentDirection;

    public enum FaceDirection {
        North,
        East,
        South,
        West
    }

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        currentDirection = (FaceDirection)stateIndex;

        switch (currentDirection) {
            case FaceDirection.North:
                transform.rotation = Quaternion.Euler(-90, 0, 0);
                break;
            case FaceDirection.East:
                transform.rotation = Quaternion.Euler(-90, 90, 0);
                break;
            case FaceDirection.South:
                transform.rotation = Quaternion.Euler(-90, 180, 0);
                break;
            case FaceDirection.West:
                transform.rotation = Quaternion.Euler(-90, 270, 0);
                break;
        }
    }

    public void Trigger() {
        GameObject orbObject = Instantiate(orbPrefab, gridLocation, orbPrefab.transform.rotation);
        MovingOrb orb = orbObject.GetComponent<MovingOrb>();
        switch (currentDirection) {
            case FaceDirection.North:
                orb.state = MovingOrb.OrbState.MovingNorth;
                break;
            case FaceDirection.East:
                orb.state = MovingOrb.OrbState.MovingEast;
                break;
            case FaceDirection.South:
                orb.state = MovingOrb.OrbState.MovingSouth;
                break;
            case FaceDirection.West:
                orb.state = MovingOrb.OrbState.MovingWest;
                break;
        }
        orb.DoMove();
    }
}
