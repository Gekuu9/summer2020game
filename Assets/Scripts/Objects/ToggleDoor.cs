using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDoor : Door, TriggerableObject {

    public bool isOpen;

    public Transform[] doors;

    public bool isLevelTransition;
    public bool updatesSurroundingBlocks;
    public bool oneTimeOpen;

    public string[] requiredFlagNames;
    public bool allFlagsRequired;

    public string openDoorName;
    public string closeDoorName;

    public Quaternion[] openRotations;
    public Quaternion[] closedRotations;

    public Vector3[] openOffsets;
    public Vector3[] closedOffsets;

    public Vector3[] openScales;
    public Vector3[] closedScales;

    public virtual void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        isOpen = stateIndex > 0;

        foreach (string flagName in requiredFlagNames) {
            if (SaveDataManager.instance.GetBoolFlag(flagName)) {
                isOpen = true;
                if (!allFlagsRequired) {
                    break;
                }
            }
            else {
                isOpen = false;
                if (allFlagsRequired) {
                    break;
                }
            }
        }

        if (GetComponent<MeshOutline>()) GetComponent<MeshOutline>().enabled = isOpen;

        for (int i = 0; i < doors.Length; i++) {
            if (isOpen) {
                doors[i].position += openOffsets[i];
                doors[i].localRotation = openRotations[i];
                doors[i].localScale = openScales[i];
            }
            else {
                doors[i].position += closedOffsets[i];
                doors[i].localRotation = closedRotations[i];
                doors[i].localScale = closedScales[i];
            }
        }

        SetupPathfinding();
    }

    public void SetupPathfinding() {
        Vector3Int location = Vector3Int.RoundToInt(gridLocation);
        for (int x = bottomCorner.x; x < topCorner.x; x++) {
            for (int y = bottomCorner.y; y < topCorner.y; y++) {
                for (int z = bottomCorner.z; z < topCorner.z; z++) {
                    Vector3Int loc2 = new Vector3Int(x, y, z);
                    Block block = LevelRenderer.instance.GetBlock(location + loc2);
                    if (block == null) continue;
                    block.ChangePathRules(isOpen && updatesSurroundingBlocks ? 1 : 0);
                }
            }
        }
        ChangePathRules(isOpen ? 1 : 0);

        for (int x = bottomCorner.x; x < topCorner.x; x++) {
            for (int y = bottomCorner.y; y < topCorner.y; y++) {
                for (int z = bottomCorner.z; z < topCorner.z; z++) {
                    Vector3Int loc2 = new Vector3Int(x, y, z);
                    Block block = LevelRenderer.instance.GetBlock(location + loc2);
                    if (block == null) continue;
                }
            }
        }
    }

    public void UpdatePathfinding() {
        if (updatesSurroundingBlocks) SetupPathfinding();
        else {
            ChangePathRules(isOpen ? 1 : 0);
        }
    }

    public void PlayAnimation() {
        if (isOpen) {
            GetComponent<Animation>().Play(openDoorName);
            transform.Find("SoundManager").GetComponent<AudioSource>().Play();
        }
        else {
            GetComponent<Animation>().Play(closeDoorName);
            transform.Find("SoundManager").GetComponent<AudioSource>().Play();
        }
    }

    public void Trigger() {
        if (oneTimeOpen && isOpen) return;
        isOpen = !isOpen;
        if (GetComponent<MeshOutline>()) GetComponent<MeshOutline>().enabled = isOpen;
        PlayAnimation();
        UpdatePathfinding();
    }

    private void Update() {
        /*
        if (transform.Find("SoundManager")) {
            if (GetComponent<Animation>().isPlaying) {
                if (!transform.Find("SoundManager").GetComponent<AudioSource>().isPlaying) {
                    transform.Find("SoundManager").GetComponent<AudioSource>().Play();
                }
            } else if (transform.Find("SoundManager").GetComponent<AudioSource>().isPlaying) {
                transform.Find("SoundManager").GetComponent<AudioSource>().Stop();
            }
        }
        */
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0) && isOpen) {
            FindPathHere();
        }
    }

    public override bool MovePlayerHere() {
        if (isOpen) {
            Vector3 location = gridLocation;
            LevelRenderer.instance.player.Move(location + movePlayerOffset, 3f);
            if (isLevelTransition)
                LevelRenderer.instance.LevelTransition(nextLevelIndex);
            return true;
        }
        return false;
    }

    public override bool FindPathHere() {
        if (isOpen) {
            Vector3 location = gridLocation;
            return LevelRenderer.instance.player.PathMove(location);
        } else {
            return false;
        }
    }
}
