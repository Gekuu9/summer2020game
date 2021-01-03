using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFadeout : MonoBehaviour {

    private static float fadeSpeed = 10f;

    [HideInInspector]
    public bool isMouseOver = false;

    /*
    private void OnMouseOver() {
        isMouseOver = true;
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
            foreach (Material material in renderer.materials) {
                if (material.color.a != 0) {
                    Color color = material.color;
                    color.a = Mathf.Lerp(color.a, 0, fadeSpeed * Time.deltaTime);
                    material.color = color;
                }
            }

        }
    }
    */

    private void Update() {
        if (isMouseOver) {
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
                foreach (Material material in renderer.materials) {
                    if (material.color.a != 0) {
                        Color color = material.color;
                        color.a -= Time.deltaTime * fadeSpeed;
                        if (color.a < 0) color.a = 0;
                        material.color = color;
                    }
                }

            }
        } else {
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
                foreach (Material material in renderer.materials) {
                    if (material.color.a != 1) {
                        Color color = material.color;
                        color.a += Time.deltaTime * fadeSpeed;
                        if (color.a > 1) color.a = 1;
                        material.color = color;
                    }
                }
            }
        }
        isMouseOver = false;
    }
}
