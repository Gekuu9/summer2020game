using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RandomTextureData")]
public class RandomTextureData : ScriptableObject {

    public TextureOption[] textureOptions;

    [Serializable]
    public class TextureOption {
        public Texture texture;
        public Texture normalMap;
        [Tooltip("Frequencies for all textures in this block should add up to 1")]
        public float frequency;
    }
}
