using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SetPiece")]
public class SetPiece : ScriptableObject {
    public BlockGrid.BlockRect[] blockRects;
    public BlockGrid.DecorationRect[] decorationRects;
    public BlockGrid.InteractableInfo[] interactables;
}
