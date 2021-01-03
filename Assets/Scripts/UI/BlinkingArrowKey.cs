using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BlinkingArrowKey : BlinkingImage {

    public float desiredAngle;
    public bool oneTime;

    private void Start() {
        if (desiredAngle < 0) desiredAngle += 360;
    }

    void Update() {
        UpdateImage();
        if (CheckAngle()) {
            if (blinking) DisableBlinking();
        } else if (!blinking && !oneTime) {
            EnableBlinking();
        }
    }

    private bool CheckAngle() {
        return Camera.main.transform.rotation.eulerAngles.y < desiredAngle + 10 && Camera.main.transform.rotation.eulerAngles.y > desiredAngle - 10;
    }
}
