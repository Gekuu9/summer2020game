using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/PathfindingRules")]
public class PathRules : ScriptableObject {

    public List<Block.Direction> inRules;

    public List<Block.Direction> outRules;
}
