using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallDoor : Door {

    private void Start() {
        Vector3Int pos = Vector3Int.RoundToInt(GetComponent<BlockInfo>().gridLocation);
        pos.y += 1;
        GameObject obj = LevelRenderer.instance.GetObject(pos);
        if (obj != null && !obj.GetComponent<Empty>()) {
            obj.SetActive(false);
        }
    }
}
