using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelDecorationSet")]
public class LevelDecorationSet : ScriptableObject {
    public GameObject[] decorationPrefabs;
}
