using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour {
    [HideInInspector]
    public Vector3 velocity = Vector3.zero;
    [HideInInspector]
    public Vector3 moveVelocity;

    //[HideInInspector]
    public Vector3 gridPosition;

    public Vector3 playerOffset;
    public Vector3 startingPosition;

    [HideInInspector]
    public int moveTimer;

    private float angleToTarget;

    [HideInInspector]
    public Vector3 moveTargetPosition;
    [HideInInspector]
    public Vector3 pathTargetPosition;

    private AStarPathfinding.APath currentPath;

    public bool gravityEnabled;

    private AudioSource playerAudio;

    [HideInInspector]
    public bool acceptingInput = true;

    public event Action onFindNewPath;

    // Start is called before the first frame update
    void Start() {
        GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
        gridPosition = startingPosition;
        transform.position = startingPosition + playerOffset;
        playerAudio = transform.Find("Sound Controller/Player Sound Controller").GetComponent<AudioSource>();

        EventHandler.instance.suppressInput += SuppressInput;
        EventHandler.instance.handleInput += HandleInput;
    }

    private void SuppressInput() {
        acceptingInput = false;
    }

    private void HandleInput() {
        acceptingInput = true;
    }

    private void Update() {
        if (moveVelocity != Vector3.zero && !playerAudio.isPlaying) {
            playerAudio.Play();
        } else if (moveVelocity == Vector3.zero && playerAudio.isPlaying) {
            playerAudio.Stop();
        }
    }

    private void FixedUpdate() {
        if (moveVelocity != Vector3.zero) {
            if (moveTimer == 0) {
                velocity -= moveVelocity;
                moveVelocity = Vector3.zero;
                gridPosition = moveTargetPosition;
                moveTargetPosition.y = transform.position.y;
                transform.position = moveTargetPosition;
            }
            else {
                moveTimer--;
            }
        }

        if (angleToTarget != 0) {
            if (angleToTarget >= 10) {
                transform.Rotate(Vector3.up, 10);
                angleToTarget -= 10;
            } else if (angleToTarget <= -10) {
                transform.Rotate(Vector3.up, -10);
                angleToTarget += 10;
            } else {
                transform.Rotate(Vector3.up, angleToTarget);
                angleToTarget = 0;
            }
        }

        if (velocity != Vector3.zero) {
            transform.Translate(velocity, Space.World);
            gridPosition = transform.position - playerOffset;
        }

        if (currentPath != null && moveTimer == 0) {
            if (currentPath.next != null) {
                if (!currentPath.next.node.block.MovePlayerHere()) currentPath = null;
                if (currentPath == null) return;
                currentPath = currentPath.next;
            } else {
                currentPath = null;
                // transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z)) + playerOffset;
            }
        }
    }

    public void Move(Vector3 location, float speed, bool useGravity = true) {
        moveTargetPosition = location;
        Vector3 distance = location - (transform.position - playerOffset);
        moveVelocity = distance.normalized * speed / 50f;
        velocity = moveVelocity;
        moveTimer = (int)(distance.magnitude / (speed / 50f));
        Vector3 forward = distance.normalized;
        forward.y = 0;
        forward.Normalize();
        angleToTarget = Mathf.Round(Vector3.SignedAngle(transform.rotation * Vector3.forward, forward, Vector3.up));
        gravityEnabled = useGravity;
        if (!gravityEnabled) {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        } else {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
    }

    public bool PathMove(Vector3 endPosition, bool findClosest = false) {
        if (!acceptingInput) return false;
        Vector3 startPosition = gridPosition;
        int startIndex = (int)startPosition.x + (int)startPosition.y * LevelRenderer.instance.levelObject.xSize + (int)startPosition.z * LevelRenderer.instance.levelObject.xSize * LevelRenderer.instance.levelObject.ySize;
        int endIndex = (int)endPosition.x + (int)endPosition.y * LevelRenderer.instance.levelObject.xSize + (int)endPosition.z * LevelRenderer.instance.levelObject.xSize * LevelRenderer.instance.levelObject.ySize;
        AStarPathfinding.APath thisPath = LevelRenderer.instance.levelObject.pathfinder.FindPath(startIndex, endIndex, findClosest);
        if (thisPath != null) {
            currentPath = thisPath;
            pathTargetPosition = endPosition;
            onFindNewPath?.Invoke();
            return true;
        } else {
            return false;
        }
    }

    public void ResetPlayerAtPosition(Vector3 position, Quaternion rotation) {
        velocity = Vector3.zero;
        moveVelocity = Vector3.zero;
        moveTimer = 0;
        angleToTarget = 0;
        currentPath = null;

        gridPosition = position;
        transform.position = position;
        transform.rotation = rotation;
    }

    public void TurnPlayer(Vector3 direction) {
        direction.y = 0;
        direction.Normalize();
        angleToTarget = Mathf.Round(Vector3.SignedAngle(transform.rotation * Vector3.forward, direction, Vector3.up));
    }

    public void StopMove() {
        currentPath = null;
        moveTimer = 0;
        velocity -= moveVelocity;
        moveVelocity = Vector3.zero;
        angleToTarget = 0;
        gridPosition = moveTargetPosition;
        moveTargetPosition.y = transform.position.y;
        transform.position = moveTargetPosition;
    }
}
