using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeWall : MonoBehaviour {

    private static float fadeSpeed = 5f;

    private void OnEnable() {
        SetFace();
    }

    private void Start() {
        SetFace();
    }

    private void Update() {
        UpdateFace();
    }

    public virtual bool CheckFacing(Transform obj) {
        return Vector3.Angle(obj.forward, Camera.main.transform.forward) < 90;
    }

    private void UpdateFace() {
        foreach (BoxCollider item in GetComponentsInChildren<BoxCollider>()) {
            if (CheckFacing(item.transform)) {
                item.enabled = false;
            }
            else {
                item.enabled = true;
            }
        }

        foreach (MeshRenderer item in GetComponentsInChildren<MeshRenderer>()) {
            if (CheckFacing(item.transform)) {
                if (item.material.color.a != 0) {
                    Color color = item.material.color;
                    color.a = Mathf.Lerp(color.a, 0, fadeSpeed * Time.deltaTime);
                    item.material.color = color;
                }
            }
            else {
                if (item.material.color.a != 1) {
                    Color color = item.material.color;
                    color.a = Mathf.Lerp(color.a, 1, fadeSpeed * Time.deltaTime);
                    item.material.color = color;
                }
            }
        }

        foreach (SpriteMask item in GetComponentsInChildren<SpriteMask>()) {
            if (CheckFacing(item.transform.parent)) {
                item.enabled = false;
            } else {
                item.enabled = true;
            }
        }
    }

    private void SetFace() {
        foreach (BoxCollider item in GetComponentsInChildren<BoxCollider>()) {
            if (CheckFacing(item.transform)) {
                item.enabled = false;
            }
            else {
                item.enabled = true;
            }
        }

        foreach (MeshRenderer item in GetComponentsInChildren<MeshRenderer>()) {
            if (CheckFacing(item.transform)) {
                Color color = item.material.color;
                color.a = 0;
                item.material.color = color;
            }
            else {
                Color color = item.material.color;
                color.a = 1;
                item.material.color = color;
            }
        }

        foreach (SpriteMask item in GetComponentsInChildren<SpriteMask>()) {
            if (CheckFacing(item.transform.parent)) {
                item.enabled = false;
            }
            else {
                item.enabled = true;
            }
        }
    }
}
