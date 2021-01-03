using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbMirror : Block, InputTriggerObject {

    public Transform pivot;

    //[HideInInspector]
    public FaceDirection currentDirection;

    public float rotateTime = 1f;

    public int testInput;

    public enum FaceDirection {
        NorthEast,
        SouthEast,
        SouthWest,
        NorthWest
    }

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        currentDirection = (FaceDirection)stateIndex;

        switch (currentDirection) {
            case FaceDirection.NorthEast:
                pivot.rotation = Quaternion.Euler(-90, 225, 0);
                break;
            case FaceDirection.SouthEast:
                pivot.rotation = Quaternion.Euler(-90, 315, 0);
                break;
            case FaceDirection.SouthWest:
                pivot.rotation = Quaternion.Euler(-90, 45, 0);
                break;
            case FaceDirection.NorthWest:
                pivot.rotation = Quaternion.Euler(-90, 135, 0);
                break;
        }

    }

    public void Trigger(int input) {
        if (input < 4) {
            currentDirection = (FaceDirection)input;

            switch (currentDirection) {
                case FaceDirection.NorthEast:
                    iTween.RotateTo(pivot.gameObject, new Vector3(-90, 225, 0), rotateTime);
                    break;
                case FaceDirection.SouthEast:
                    iTween.RotateTo(pivot.gameObject, new Vector3(-90, 315, 0), rotateTime);
                    break;
                case FaceDirection.SouthWest:
                    iTween.RotateTo(pivot.gameObject, new Vector3(-90, 45, 0), rotateTime);
                    break;
                case FaceDirection.NorthWest:
                    iTween.RotateTo(pivot.gameObject, new Vector3(-90, 135, 0), rotateTime);
                    break;
            }
        } else {
            if (input == 5) {
                currentDirection = (FaceDirection)(((int)currentDirection + 1) % 4);
                iTween.RotateAdd(pivot.gameObject, new Vector3(0, -90, 0), rotateTime);
            } else if (input == 6) {
                currentDirection = (FaceDirection)Mathf.Abs(((int)currentDirection - 1) % 4);
                iTween.RotateAdd(pivot.gameObject, new Vector3(0, 90, 0), rotateTime);
            }
        }
    }

    public void Trigger() {
        if (testInput < 4) {
            currentDirection = (FaceDirection)testInput;

            Hashtable args = new Hashtable();
            args.Add("easetype", iTween.EaseType.linear);
            args.Add("time", rotateTime);
            iTween.RotateAdd(pivot.gameObject, args);
            switch (currentDirection) {
                case FaceDirection.NorthEast:
                    args.Add("rotation", new Vector3(-90, 225, 0));
                    break;
                case FaceDirection.SouthEast:
                    args.Add("rotation", new Vector3(-90, 315, 0));
                    break;
                case FaceDirection.SouthWest:
                    args.Add("rotation", new Vector3(-90, 45, 0));
                    break;
                case FaceDirection.NorthWest:
                    args.Add("rotation", new Vector3(-90, 135, 0));
                    break;
            }
            iTween.RotateTo(pivot.gameObject, args);
        } else {
            if (testInput == 4) {
                currentDirection = (FaceDirection)(((int)currentDirection + 1) % 4);
                Hashtable args = new Hashtable();
                args.Add("easetype", iTween.EaseType.linear);
                args.Add("time", rotateTime);
                args.Add("amount", new Vector3(0, 0, 90));
                iTween.RotateAdd(pivot.gameObject, args);
            } else if (testInput == 5) {
                currentDirection = (FaceDirection)Mathf.Abs(((int)currentDirection - 1) % 4);
                Hashtable args = new Hashtable();
                args.Add("easetype", iTween.EaseType.linear);
                args.Add("time", rotateTime);
                args.Add("amount", new Vector3(0, 0, -90));
                iTween.RotateAdd(pivot.gameObject, args);
            }
        }
    }
}
