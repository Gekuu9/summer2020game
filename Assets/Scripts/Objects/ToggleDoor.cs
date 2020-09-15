using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDoor : Door, TriggerableObject {

    public bool state;

    public Transform[] doors;

    public bool isLevelTransition;
    public bool updatesSurroundingBlocks;

    public string openDoorName;
    public string closeDoorName;

    public Quaternion[] openRotations;
    public Quaternion[] closedRotations;

    public Vector3[] openOffsets;
    public Vector3[] closedOffsets;

    public Vector3[] openScales;
    public Vector3[] closedScales;

    public Vector3Int bottomCorner;
    public Vector3Int topCorner;

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        state = stateIndex > 0;

        for (int i = 0; i < doors.Length; i++) {
            if (state) {
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
        PFRulesVariable.variantRuleset rules = GetComponent<PFRulesVariable>().variantRules[state && updatesSurroundingBlocks ? 1 : 0];
        Vector3Int location = Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation);
        for (int x = bottomCorner.x; x < topCorner.x; x++) {
            for (int y = bottomCorner.y; y < topCorner.y; y++) {
                for (int z = bottomCorner.z; z < topCorner.z; z++) {
                    Vector3Int loc2 = new Vector3Int(x, y, z);
                    GameObject obj = LevelRenderer.instance.GetObject(location + loc2);
                    if (obj == null) continue;
                    obj.GetComponent<PFRulesVariable>().UseRuleset(rules);
                }
            }
        }
        GetComponent<PFRulesVariable>().SwitchRuleset(state ? 1 : 0);

        for (int x = bottomCorner.x; x < topCorner.x; x++) {
            for (int y = bottomCorner.y; y < topCorner.y; y++) {
                for (int z = bottomCorner.z; z < topCorner.z; z++) {
                    Vector3Int loc2 = new Vector3Int(x, y, z);
                    GameObject obj = LevelRenderer.instance.GetObject(location + loc2);
                    if (obj == null) continue;
                    obj.GetComponent<PFRulesVariable>().UpdatePFGraph();
                }
            }
        }
    }

    public void UpdatePathfinding() {
        if (updatesSurroundingBlocks) SetupPathfinding();
        else {
            GetComponent<PFRulesVariable>().SwitchRuleset(state ? 1 : 0);
            GetComponent<PFRulesVariable>().UpdatePFGraph();
        }
    }

    public void PlayAnimation() {
        if (state) {
            GetComponent<Animation>().Play(openDoorName);
            transform.Find("SoundManager").GetComponent<AudioSource>().Play();
        }
        else {
            GetComponent<Animation>().Play(closeDoorName);
            transform.Find("SoundManager").GetComponent<AudioSource>().Play();
        }
    }

    public void Trigger() {
        state = !state;
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

    private void OnMouseEnter() {
        if (GetComponentInChildren<OutlineOrtho>() == null) return;
        if (state) {
            GetComponentInChildren<OutlineOrtho>().enabled = true;
        }
    }

    private void OnMouseExit() {
        if (GetComponentInChildren<OutlineOrtho>() == null) return;
        GetComponentInChildren<OutlineOrtho>().enabled = false;
    }

    private void OnEnable() {
        if (GetComponentInChildren<OutlineOrtho>() == null) return;
        GetComponentInChildren<OutlineOrtho>().enabled = false;
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0) && state) {
            FindPathHere();
        }
    }

    public override void MovePlayerHere() {
        if (state) {
            Vector3 location = GetComponent<BlockInfo>().gridLocation;
            LevelRenderer.instance.player.Move(location + movePlayerOffset, 3f);
            if (isLevelTransition)
                LevelRenderer.instance.LevelTransition(nextLevelIndex);
        }
    }

    public override bool FindPathHere() {
        if (state) {
            Vector3 location = GetComponent<BlockInfo>().gridLocation;
            return LevelRenderer.instance.player.PathMove(location);
        } else {
            return false;
        }
    }
}
