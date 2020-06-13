using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : ScriptableObject {
    public enum Type {
        Wall,
        InvisWall,
        Floor,
        Empty,
        Object
    }

    public abstract bool isSolid { get; set; }

    public abstract Type type { get; set;  }

    public abstract Mesh mesh { get; set;  }
}
