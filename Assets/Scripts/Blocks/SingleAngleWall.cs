using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAngleWall : Wall {

    public float angle = 45;
    public override bool CheckFacing(Transform obj) {
        return !(Camera.main.transform.rotation.eulerAngles.y > angle - 45 && Camera.main.transform.rotation.eulerAngles.y < angle + 45);
    }
}
