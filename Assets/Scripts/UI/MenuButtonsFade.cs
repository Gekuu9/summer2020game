using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonsFade : MonoBehaviour {

    public GameObject[] buttonObjects;

    private void Awake() {
        LogoFadeController.instance.onLogoFadeIn += FadeIn;
    }

    private void FadeIn() {
        GetComponent<Animation>().Play();

        foreach (GameObject button in buttonObjects) {
            button.SetActive(true);
        }
    }
}
