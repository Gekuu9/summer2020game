using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public abstract class MultiBlockHandler : BlockHandler {
    public Vector3Int corner0;
    public Vector3Int corner1;

    public virtual void SetupMultiBlock() {
        for (int x = corner0.x; x < corner1.x; x++) {
            for (int y = corner0.y; y < corner1.y; y++) {
                for (int z = corner0.z; z < corner1.z; z++) {
                    Vector3Int position = new Vector3Int(x, y, z);
                    LevelRenderer.instance.levelObject.SetObject(position, gameObject);
                }
            }
        }
    }
}
