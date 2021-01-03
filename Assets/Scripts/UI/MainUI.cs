using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {

    // The speed at which the screen will fade in and out in level transitions
    public float fadeSpeed;

    // The image to fade in and out during level transitions
    public Image fadeImage;

    // Whether the screen is currently fading in or out
    [HideInInspector]
    public FadeStatus status;

    public enum FadeStatus {
        clear,
        black,
        fadingIn,
        fadingOut
    }

    public IEnumerator LevelFadeOut() {
        status = FadeStatus.fadingOut;
        while (status == FadeStatus.fadingOut) {
            fadeImage.color = Color.Lerp(fadeImage.color, Color.black, fadeSpeed * Time.deltaTime);

            if (fadeImage.color.a >= 0.99) {
                fadeImage.color = Color.black;
                status = FadeStatus.black;
            } else {
                yield return null;
            }
        }
    }

    public IEnumerator LevelFadeOut(float fadeMultiplier) {
        status = FadeStatus.fadingOut;
        while (status == FadeStatus.fadingOut) {
            fadeImage.color = Color.Lerp(fadeImage.color, Color.black, fadeSpeed * fadeMultiplier * Time.deltaTime);

            if (fadeImage.color.a >= 0.99) {
                fadeImage.color = Color.black;
                status = FadeStatus.black;
            } else {
                yield return null;
            }
        }
    }

    public IEnumerator LevelFadeIn() {
        status = FadeStatus.fadingIn;
        while (status == FadeStatus.fadingIn) {
            fadeImage.color = Color.Lerp(fadeImage.color, Color.clear, fadeSpeed * Time.deltaTime);

            if (fadeImage.color.a <= 0.01f) {
                fadeImage.color = Color.clear;
                status = FadeStatus.clear;
            } else {
                yield return null;
            }
        }
    }

    public IEnumerator LevelFadeIn(float fadeMultiplier) {
        status = FadeStatus.fadingIn;
        while (status == FadeStatus.fadingIn) {
            fadeImage.color = Color.Lerp(fadeImage.color, Color.clear, fadeSpeed * fadeMultiplier * Time.deltaTime);

            if (fadeImage.color.a <= 0.01f) {
                fadeImage.color = Color.clear;
                status = FadeStatus.clear;
            } else {
                yield return null;
            }
        }
    }

    public IEnumerator LevelFadeInLinear(float fadeTime) {
        status = FadeStatus.fadingIn;
        while (status == FadeStatus.fadingIn) {
            Color color = fadeImage.color;
            color.a -= Time.deltaTime / fadeTime;
            if (color.a <= 0.01) {
                color.a = 0;
                status = FadeStatus.clear;
            }
                
            fadeImage.color = color;
            yield return null;
        }
    }
}
