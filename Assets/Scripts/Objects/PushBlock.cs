using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PushBlock : Floor, InteractableObject {

    public float pushSpeed;

    private Vector3 targetLocation;
    private Vector3 targetDirection;

    [HideInInspector]
    public bool moving;

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {

    }

    public void Push(Vector3 direction) {
        Vector3Int path = Vector3Int.RoundToInt(transform.position + direction);
        if (!CheckPath(path)) return;

        LevelRenderer.instance.GetBlock(path).ChangePathRules(0);
        Vector3Int aboveLoc = gridLocation;
        aboveLoc.y += 1;
        if (LevelRenderer.instance.GetBlock(aboveLoc).GetComponent<Empty>()) {
            LevelRenderer.instance.GetBlock(aboveLoc).ChangePathRules(0);
        }

        targetLocation = transform.position + direction;
        targetDirection = direction.normalized;

        moving = true;
        foreach (MeshOutline item in GetComponentsInChildren<MeshOutline>()) {
            item.enabled = false;
        }

        transform.GetComponentInChildren<AudioSource>().Play();
    }

    private bool CheckPath(Vector3Int path) {
        path.y -= 1;

        // Check if the block below the destination is in a valid location and is of type PushPath
        if (path.x < 0 || path.y < 0 || path.z < 0 ||
            path.x >= LevelRenderer.instance.levelObject.xSize ||
            path.y >= LevelRenderer.instance.levelObject.ySize ||
            path.z >= LevelRenderer.instance.levelObject.zSize ||
            !LevelRenderer.instance.GetBlock(path) ||
            !LevelRenderer.instance.GetBlock(path).GetComponent<PushPath>()) {

            return false;
        }

        return true;
    }

    private void Update() {
        if (moving) {
            if (transform.position != targetLocation) {
                if (Vector3.Distance(transform.position, targetLocation) <= pushSpeed * Time.deltaTime || transform.position + targetDirection * pushSpeed * Time.deltaTime == targetLocation) {
                    ResolvePush();
                } else {
                    transform.position += targetDirection * pushSpeed * Time.deltaTime;
                }
            } else {
                moving = false;
                foreach (MeshOutline item in GetComponentsInChildren<MeshOutline>()) {
                    item.enabled = true;
                }
            }
        }
    }

    private void ResolvePush() {
        LevelRenderer.instance.MoveBlock(gridLocation, Vector3Int.RoundToInt(targetLocation));
        transform.position = targetLocation;
    }
}
