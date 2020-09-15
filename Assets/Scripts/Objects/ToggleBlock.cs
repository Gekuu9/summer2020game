using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBlock : BlockHandler, TriggerableObject {

    public bool state;

    public Transform movingBlock;

    public string toggleOnName;
    public string toggleOffName;

    public Quaternion onRotation;
    public Quaternion offRotation;

    public Vector3 onOffset;
    public Vector3 offOffset;

    public Vector3 onScale;
    public Vector3 offScale;

    public override bool surfaceAbove {
        get { return state; }
    }

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        state = stateIndex > 0;

        if (state) {
            movingBlock.position += onOffset;
            movingBlock.rotation = onRotation;
            movingBlock.localScale = onScale;
        }
        else {
            movingBlock.position += offOffset;
            movingBlock.rotation = offRotation;
            movingBlock.localScale = offScale;
        }

        UpdatePathfinding();
    }

    public void UpdatePathfinding() {
        GetComponent<PFRulesVariable>().SwitchRuleset(state ? 1 : 0);
        Vector3Int location = Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation);
        if (location.y + 1 < LevelRenderer.instance.levelObject.ySize) {
            location.y += 1;
            GameObject aboveObj = LevelRenderer.instance.GetObject(location);
            if (aboveObj.GetComponent<Empty>()) {
                aboveObj.GetComponent<Empty>().UpdatePathfinding();
                aboveObj.GetComponent<PFRulesVariable>().UpdatePFGraph();
            }
        }
        GetComponent<PFRulesVariable>().UpdatePFGraph();
    }

    public void PlayAnimation() {
        if (state) {
            GetComponent<Animation>().Play(toggleOnName);
        }
        else {
            GetComponent<Animation>().Play(toggleOffName);
        }
    }

    public void Trigger() {
        state = !state;
        PlayAnimation();
        UpdatePathfinding();
    }

    public override void MovePlayerHere() {
        if (!state) {
            Vector3 location = GetComponent<BlockInfo>().gridLocation;
            LevelRenderer.instance.player.Move(location, 3f);
        } else {
            Vector3Int position = Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation);
            position.y += 1;
            LevelRenderer.instance.GetObject(position).GetComponent<BlockHandler>().FindPathHere();
        }
    }

    public override bool FindPathHere() {
        if (!state) {
            Vector3 location = GetComponent<BlockInfo>().gridLocation;
            return LevelRenderer.instance.player.PathMove(location);
        } else {
            Vector3Int position = Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation);
            position.y += 1;
            return LevelRenderer.instance.GetObject(position).GetComponent<BlockHandler>().FindPathHere();
        }
    }
}
