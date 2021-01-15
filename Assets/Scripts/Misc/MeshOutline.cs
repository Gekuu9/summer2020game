using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshOutline : MonoBehaviour {

    public GameObject[] extraObjects;
    public bool toggleChildren;

    [HideInInspector]
    public bool outlineEnabled;

    [HideInInspector]
    public bool allowOutline = true;

    private void OnMouseEnter() {
        SetOutline(true);
    }

    private void OnMouseExit() {
        SetOutline(false);
    }

    private void OnEnable() {
        SetOutline(false);
    }

    private void OnDisable() {
        SetOutline(false);
    }

    private void OnMouseOver() {
        if (!outlineEnabled) {
            SetOutline(true);
        }
    }

    private void Start() {
        EventHandler.instance.suppressInput += DisallowOutline;
        EventHandler.instance.handleInput += AllowOutline;
        allowOutline = true;
    }

    private void SetOutline(bool b) {
        if (!enabled || !allowOutline) b = false;
        if (GetComponent<OutlineOrthoSingle>()) GetComponent<OutlineOrthoSingle>().enabled = b;
        if (GetComponent<OutlineOrtho>()) GetComponent<OutlineOrtho>().enabled = b;
        if (toggleChildren) {
            foreach (OutlineOrthoSingle outline in GetComponentsInChildren<OutlineOrthoSingle>()) {
                outline.enabled = b;
            }
            foreach (OutlineOrtho outline in GetComponentsInChildren<OutlineOrtho>()) {
                outline.enabled = b;
            }
        }
        foreach (GameObject o in extraObjects) {
            o.SetActive(b);
        }

        outlineEnabled = b;
    }

    private void DisallowOutline() {
        allowOutline = false;
    }

    private void AllowOutline() {
        allowOutline = true;
    }
}
