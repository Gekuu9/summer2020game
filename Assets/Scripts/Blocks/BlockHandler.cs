using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockHandler : MonoBehaviour {
    [HideInInspector]
    public virtual bool surfaceAbove {
        get { return false; }
    }

    public abstract void MovePlayerHere();

    public abstract bool FindPathHere();
}
