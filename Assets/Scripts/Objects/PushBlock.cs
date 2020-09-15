using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlock : Floor, InteractableObject {

    public float pushSpeed;

    private Vector3 targetLocation;
    private Vector3 targetDirection;

    [HideInInspector]
    public bool moving;

    public void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        LevelRenderer.instance.levelObject.CreateBlockRect(GetComponent<BlockInfo>().gridLocation, GetComponent<BlockInfo>().gridLocation + Vector3.one, BlockGrid.BlockType.Empty);
        LevelRenderer.instance.levelObject.pathfindingGraph.UpdateNode(Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation), gameObject);
    }

    public void Push(Vector3 direction) {
        Vector3Int path = Vector3Int.RoundToInt(transform.position + direction);
        path.y -= 1;
        if (path.x < 0 || path.y < 0 || path.z < 0 || path.x >= LevelRenderer.instance.levelObject.xSize || path.y >= LevelRenderer.instance.levelObject.ySize || path.z >= LevelRenderer.instance.levelObject.zSize) return;
        if (!LevelRenderer.instance.GetObject(path) || !LevelRenderer.instance.GetObject(path).GetComponent<PushPath>()) return;
        path.y += 1;
        LevelRenderer.instance.GetObject(path).GetComponent<PFRulesVariable>().SwitchRuleset(0);
        Vector3Int aboveLoc = Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation);
        aboveLoc.y += 1;
        if (LevelRenderer.instance.GetObject(aboveLoc).GetComponent<Empty>()) {
            LevelRenderer.instance.GetObject(aboveLoc).GetComponent<PFRulesVariable>().SwitchRuleset(0);
            LevelRenderer.instance.GetObject(aboveLoc).GetComponent<PFRulesVariable>().UpdatePFGraph();
        }
        LevelRenderer.instance.GetObject(path).GetComponent<PFRulesVariable>().UpdatePFGraph();

        targetLocation = transform.position + direction;
        targetDirection = direction.normalized;

        moving = true;

        transform.GetComponentInChildren<AudioSource>().Play();
    }

    private void Update() {
        if (moving) {
            if (transform.position != targetLocation) {
                if (Vector3.Distance(transform.position, targetLocation) <= pushSpeed || transform.position + targetDirection * pushSpeed == targetLocation) {
                    ResolvePush();
                } else {
                    transform.position += targetDirection * pushSpeed;
                }
            } else {
                moving = false;
            }
        }
    }

    private void ResolvePush() {
        transform.position = targetLocation;

        GameObject oldEmpty = LevelRenderer.instance.GetObject(Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation));
        oldEmpty.GetComponent<PFRulesVariable>().SwitchRuleset(1);

        GameObject empty = LevelRenderer.instance.GetObject(Vector3Int.RoundToInt(targetLocation));
        GetComponent<BlockInfo>().gridLocation = empty.GetComponent<BlockInfo>().gridLocation;
        
        Vector3 above = GetComponent<BlockInfo>().gridLocation;
        above.y += 1;
        if (LevelRenderer.instance.GetObject(Vector3Int.RoundToInt(above)) == null) {
            LevelRenderer.instance.levelObject.CreateBlockRect(above, above + Vector3.one, BlockGrid.BlockType.Surface);
        } else {
            LevelRenderer.instance.GetObject(Vector3Int.RoundToInt(above)).GetComponent<PFRulesVariable>().SwitchRuleset(1);
            LevelRenderer.instance.levelObject.pathfindingGraph.UpdateNode(Vector3Int.RoundToInt(above));
        }

        LevelRenderer.instance.levelObject.pathfindingGraph.UpdateNode(Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation), gameObject);
        LevelRenderer.instance.levelObject.pathfindingGraph.UpdateNode(Vector3Int.RoundToInt(oldEmpty.GetComponent<BlockInfo>().gridLocation), oldEmpty);
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
}
