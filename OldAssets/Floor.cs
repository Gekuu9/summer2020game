using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Floor : Block {
    public bool _isSolid = true;
    public Type _type = Type.Floor;
    public Mesh _mesh;

    public override bool isSolid { get { return _isSolid; } set { _isSolid = value; } }
    public override Type type { get { return _type; } set { _type = value; } }
    public override Mesh mesh { get { return _mesh; } set { _mesh = value; } }

    public Floor(Mesh mesh) {
        _mesh = mesh;
    }
}
