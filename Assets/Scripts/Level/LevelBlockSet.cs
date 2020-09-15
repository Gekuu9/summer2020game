using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelBlockSet")]
public class LevelBlockSet : ScriptableObject {
    public GameObject[] blockPrefabs;
}
