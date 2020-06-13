using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour {
    private float g = -0.1f;
    private Vector3 velocity = Vector3.zero;

    private Vector3 gridPosition;

    public Vector3 playerOffset;
    public Vector3 startingPosition;

    private int moveTimer;
    private Vector3 moveVelocity;

    private AStarPathfinding.APath currentPath;

    // Start is called before the first frame update
    void Start() {
        gridPosition = startingPosition;
        transform.position = startingPosition + playerOffset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        transform.Translate(velocity);
        //velocity.y += g / 50f;
        if (moveTimer > 0) {
            moveTimer--;
            transform.Translate(moveVelocity);
        }
        gridPosition = transform.position - playerOffset;

        if (currentPath != null && moveTimer == 0) {
            if (currentPath.next != null) {
                currentPath.next.node.obj.GetComponent<BlockHandler>().MovePlayerHere();
                currentPath = currentPath.next;
            } else {
                currentPath = null;
                transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z)) + playerOffset;
            }
        }
    }

    public void Move(Vector3 location, float speed) {
        Vector3 distance = location - (transform.position - playerOffset);
        moveVelocity = distance.normalized * speed / 50f;
        moveTimer = (int)(distance.magnitude / (speed / 50f));
    }

    public void PathMove(Vector3 endPosition) {
        Vector3 startPosition = transform.position - playerOffset;
        int startIndex = (int)startPosition.x + (int)startPosition.y * LevelRenderer.instance.blockGrid.xSize + (int)startPosition.z * LevelRenderer.instance.blockGrid.xSize * LevelRenderer.instance.blockGrid.ySize;
        int endIndex = (int)endPosition.x + (int)endPosition.y * LevelRenderer.instance.blockGrid.xSize + (int)endPosition.z * LevelRenderer.instance.blockGrid.xSize * LevelRenderer.instance.blockGrid.ySize;
        AStarPathfinding.APath thisPath = LevelRenderer.instance.pathfinder.FindPath(startIndex, endIndex);
        if (thisPath != null) {
            currentPath = thisPath;
        }
    }
}
