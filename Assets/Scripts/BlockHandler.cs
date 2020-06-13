using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockHandler : MonoBehaviour {
    public abstract void MovePlayerHere();

    public abstract void FindPathHere();
}
