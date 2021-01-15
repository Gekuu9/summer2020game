using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbMirror : Block, InputTriggerObject {

    public GameObject pivot;

    [HideInInspector]
    public FaceDirection currentDirection;

    public float rotateTime = 1f;

    public List<FaceDirection> faceDirections;

    [HideInInspector]
    public bool currentlyRotating = false;

    public enum FaceDirection {
        NorthEast,
        SouthEast,
        SouthWest,
        NorthWest
    }

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        UpdateFaceDirection(stateIndex);
        if (!faceDirections.Contains(currentDirection)) {
            Debug.LogError("Mirror at " + gridLocation + " given invalid initial direction " + currentDirection);
            return;
        }

        switch (currentDirection) {
            case FaceDirection.NorthEast:
                pivot.transform.rotation = Quaternion.Euler(-90, 225, 0);
                break;
            case FaceDirection.SouthEast:
                pivot.transform.rotation = Quaternion.Euler(-90, 315, 0);
                break;
            case FaceDirection.SouthWest:
                pivot.transform.rotation = Quaternion.Euler(-90, 45, 0);
                break;
            case FaceDirection.NorthWest:
                pivot.transform.rotation = Quaternion.Euler(-90, 135, 0);
                break;
        }

    }

    public void Trigger(int input) {
        if (input > 1) {
            UpdateFaceDirection(input - 2);
            if (!faceDirections.Contains(currentDirection)) {
                Debug.LogError("Mirror at " + gridLocation + " given invalid direction " + currentDirection);
                return;
            }

            switch (currentDirection) {
                case FaceDirection.NorthEast:
                    iTween.RotateTo(pivot, new Vector3(-90, 225, 0), rotateTime);
                    break;
                case FaceDirection.SouthEast:
                    iTween.RotateTo(pivot, new Vector3(-90, 315, 0), rotateTime);
                    break;
                case FaceDirection.SouthWest:
                    iTween.RotateTo(pivot, new Vector3(-90, 45, 0), rotateTime);
                    break;
                case FaceDirection.NorthWest:
                    iTween.RotateTo(pivot, new Vector3(-90, 135, 0), rotateTime);
                    break;
            }
        } else {
            if (input == 0) {
                UpdateFaceDirection(((int)currentDirection + 1) % 4);
                iTween.RotateAdd(pivot, new Vector3(0, -90, 0), rotateTime);
            } else if (input == 1) {
                UpdateFaceDirection(Mathf.Abs(((int)currentDirection - 1) % 4));
                iTween.RotateAdd(pivot, new Vector3(0, 90, 0), rotateTime);
            }
        }
    }

    public void Trigger() {
        if (currentlyRotating) return;
        currentlyRotating = true;
        int newDirection = ((int)currentDirection + 1) % faceDirections.Count;
        UpdateFaceDirection(newDirection);
        Hashtable args = new Hashtable();
        args.Add("easetype", iTween.EaseType.linear);
        args.Add("time", rotateTime);
        args.Add("amount", new Vector3(0, 0, 90));
        args.Add("oncomplete", "FinishRotate");
        args.Add("oncompletetarget", gameObject);
        iTween.RotateAdd(pivot, args);
    }

    public virtual void UpdateFaceDirection(int newDirection) {
        currentDirection = faceDirections[newDirection];
    }

    public void FinishRotate() {
        currentlyRotating = false;
    }
}
