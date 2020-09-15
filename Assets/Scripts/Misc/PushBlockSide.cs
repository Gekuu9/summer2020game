using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PushBlockSide : MonoBehaviour {

    public Vector3 direction;

    private bool waitingForPlayer;

    private void Update() {
        if (waitingForPlayer) {
            Vector3 location = GetComponentInParent<BlockInfo>().gridLocation - direction;
            if (LevelRenderer.instance.player.pathTargetPosition != location) {
                waitingForPlayer = false;
            }
            else if (LevelRenderer.instance.player.gridPosition == location) {
                waitingForPlayer = false;
                LevelRenderer.instance.player.TurnPlayer(direction);
                GetComponentInParent<PushBlock>().Push(direction);
                transform.GetChild(0).gameObject.SetActive(false);
                GetComponent<OutlineOrthoSingle>().enabled = false;
            }
        }
    }

    private void OnMouseEnter() {
        if (!GetComponentInParent<PushBlock>().moving) {
            transform.GetChild(0).gameObject.SetActive(true);
            GetComponent<OutlineOrthoSingle>().enabled = true;
        }
    }

    private void OnMouseExit() {
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<OutlineOrthoSingle>().enabled = false;
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0) && !GetComponentInParent<PushBlock>().moving) {
            if (LevelRenderer.instance.player.gridPosition == GetComponentInParent<BlockInfo>().gridLocation - direction) {
                LevelRenderer.instance.player.TurnPlayer(direction);
                GetComponentInParent<PushBlock>().Push(direction);
                transform.GetChild(0).gameObject.SetActive(false);
                GetComponent<OutlineOrthoSingle>().enabled = false;
                return;
            }
            if (!FindPathHere()) return;
            waitingForPlayer = true;
        }

        if (GetComponent<OutlineOrthoSingle>().enabled == false && !GetComponentInParent<PushBlock>().moving) {
            transform.GetChild(0).gameObject.SetActive(true);
            GetComponent<OutlineOrthoSingle>().enabled = true;
        }
    }

    public bool FindPathHere() {
        Vector3 location = GetComponentInParent<BlockInfo>().gridLocation - direction;
        return LevelRenderer.instance.player.PathMove(location);
    }


}
