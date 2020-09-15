using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class RandomizeTexture : MonoBehaviour {

    public RandomTextureData randomTextureData;

    private void Awake() {
        int seed;

        if (GetComponent<BlockInfo>()) {
            seed = transform.GetSiblingIndex();
        } else {
            seed = transform.parent.GetSiblingIndex() * (1 + transform.GetSiblingIndex());
        }
        UnityEngine.Random.InitState(seed);
        float val = UnityEngine.Random.Range(0f, 1f);
        float total = 0;
        RandomTextureData.TextureOption selected = null;
        foreach (RandomTextureData.TextureOption item in randomTextureData.textureOptions) {
            total += item.frequency;
            if (total >= val) {
                selected = item;
                break;
            }
        }

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material.mainTexture = selected.texture;
        renderer.material.SetTexture("_BumpMap", selected.normalMap);
    }
}
