using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {

    // The speed at which the screen will fade in and out in level transitions
    public float fadeSpeed;

    // The image to fade in and out during level transitions
    public Image fadeImage;

    // Whether the screen is currently fading in or out
    [HideInInspector]
    public bool fading;

    public IEnumerator LevelFadeOut() {
        fading = true;
        while (fading) {
            fadeImage.color = Color.Lerp(fadeImage.color, Color.black, fadeSpeed * Time.deltaTime);

            if (fadeImage.color.a >= 0.99) {
                fadeImage.color = Color.black;
                fading = false;
            } else {
                yield return null;
            }
        }
    }

    public IEnumerator LevelFadeOut(float fadeMultiplier) {
        fading = true;
        while (fading) {
            fadeImage.color = Color.Lerp(fadeImage.color, Color.black, fadeSpeed * fadeMultiplier * Time.deltaTime);

            if (fadeImage.color.a >= 0.99) {
                fadeImage.color = Color.black;
                fading = false;
            }
            else {
                yield return null;
            }
        }
    }

    public IEnumerator LevelFadeIn() {
        fading = true;
        while (fading) {
            fadeImage.color = Color.Lerp(fadeImage.color, Color.clear, fadeSpeed * Time.deltaTime);

            if (fadeImage.color.a <= 0.01f) {
                fadeImage.color = Color.clear;
                fading = false;
            }
            else {
                yield return null;
            }
        }
    }

    public IEnumerator LevelFadeIn(float fadeMultiplier) {
        fading = true;
        while (fading) {
            fadeImage.color = Color.Lerp(fadeImage.color, Color.clear, fadeSpeed * fadeMultiplier * Time.deltaTime);

            if (fadeImage.color.a <= 0.01f) {
                fadeImage.color = Color.clear;
                fading = false;
            }
            else {
                yield return null;
            }
        }
    }
}
