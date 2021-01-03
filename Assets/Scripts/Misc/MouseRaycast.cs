using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseRaycast : MonoBehaviour {
    private void Update() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, -1, QueryTriggerInteraction.Collide)) {
            if (hit.transform.GetComponent<RaycastFadeout>()) {
                hit.transform.GetComponent<RaycastFadeout>().isMouseOver = true;
            }
        }
    }
}
