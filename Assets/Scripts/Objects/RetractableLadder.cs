using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Security.AccessControl;
using UnityEngine;

public class RetractableLadder : Ladder, TriggerableObject {

    [HideInInspector]
    public bool state;

    public int ladderHeight;
    public int retractHeight;

    public Transform ladderModel;

    public GameObject ladderPrefab;

    public string moveUpName;
    public string moveDownName;

    [HideInInspector]
    public RetractableLadder up;
    [HideInInspector]
    public RetractableLadder down;

    [HideInInspector]
    public bool showModel;
    [HideInInspector]
    public bool climbable;

    [HideInInspector]
    public bool isSurface;
    [HideInInspector]
    public bool forceSurface = false;

    [HideInInspector]
    public int ladderIndex;

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        state = stateIndex > 0;

        GameObject obj;
        RetractableLadder objLadder = this;
        objLadder.ladderIndex = 0;
        SetupLadder();
        if (!state) ladderModel.Translate(0, 0, retractHeight);

        for (int i = 1; i < ladderHeight + retractHeight; i++) {
            GameObject obj2 = Instantiate(ladderPrefab, transform.parent);
            RetractableLadder objLadder2 = obj2.GetComponent<RetractableLadder>();

            Vector3 position = transform.position;
            position.y += i;
            obj2.transform.position = position;
            obj2.GetComponent<BlockInfo>().gridLocation = GetComponent<BlockInfo>().gridLocation;
            obj2.GetComponent<BlockInfo>().gridLocation.y += i;
            objLadder2.ladderIndex = i;

            objLadder2.SetupLadder();

            objLadder.up = obj2.GetComponent<RetractableLadder>();
            obj2.GetComponent<RetractableLadder>().down = objLadder;

            obj = obj2;
            objLadder = obj.GetComponent<RetractableLadder>();
        }
    }

    public void SetupLadder() {
        showModel = ladderIndex < ladderHeight;

        LevelRenderer.instance.levelObject.SetObject(Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation), gameObject);
        UpdatePathfinding();
        UpdateModel();
    }

    public void UpdatePathfinding() {
        if (ladderIndex < retractHeight) climbable = state;
        else if (ladderIndex >= ladderHeight) climbable = !state;
        else climbable = true;

        GetComponent<PFRulesVariable>().SwitchRuleset(climbable ? 1 : 0);

        if (!climbable) {
            Vector3 location = GetComponent<BlockInfo>().gridLocation;
            if (location.y <= 0 && !forceSurface) return;
            location.y -= 1;
            GameObject obj = LevelRenderer.instance.GetObject(Vector3Int.RoundToInt(location));
            if ((obj != null && obj.GetComponent<BlockHandler>() != null && obj.GetComponent<BlockHandler>().surfaceAbove) || forceSurface) {
                GetComponent<PFRulesVariable>().SwitchRuleset(2);
                isSurface = true;
            }
            else {
                GetComponent<PFRulesVariable>().SwitchRuleset(0);
                isSurface = false;
            }
        }

        LevelRenderer.instance.levelObject.pathfindingGraph.UpdateNode(Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation), gameObject);
    }

    public void UpdateModel() {
        if (!showModel) {
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }

    public void PlayAnimation() {
        if (state) {
            GetComponent<Animation>().Play(moveDownName);
        }
        else {
            GetComponent<Animation>().Play(moveUpName);
        }
    }

    public void Trigger() {
        state = !state;
        PlayAnimation();
        UpdatePathfinding();
        if (up != null && up.state != state) up.Trigger();
        if (down != null && down.state != state) down.Trigger();
    }

    private void OnMouseEnter() {
        GetComponentInChildren<OutlineOrthoSingle>().enabled = true;
    }

    private void OnMouseExit() {
        GetComponentInChildren<OutlineOrthoSingle>().enabled = false;
    }

    private void OnEnable() {
        GetComponentInChildren<OutlineOrthoSingle>().enabled = false;
    }

    public void TraverseUp(int index) {
        if (index == 0) {
            FindPathHere();
        } else {
            up.TraverseUp(index - 1);
        }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            if (state) {
                FindPathHere();
            } else {
                TraverseUp(retractHeight);
            }
        }
    }

    public override void MovePlayerHere() {
        if (!LevelRenderer.instance.levelObject.pathfindingGraph.CheckMove(Vector3Int.RoundToInt(LevelRenderer.instance.player.gridPosition), Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation))) {
            LevelRenderer.instance.player.StopMove();
            return;
        }
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        LevelRenderer.instance.player.Move(location, 2f, false);
        if (LevelRenderer.instance.player.gridPosition.y < location.y) LevelRenderer.instance.player.TurnPlayer(faceDirection);
    }
}
