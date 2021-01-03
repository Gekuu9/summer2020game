using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingImage : MonoBehaviour {
    public float blinkSpeed;
    public Color fullColor = Color.white;

    private bool fadeIn = true;
    protected bool blinking = true;

    void Update() {
        UpdateImage();
    }

    protected void UpdateImage() {
        Color color = GetComponent<Image>().color;
        if (fadeIn) {
            color = Color.Lerp(color, fullColor, blinkSpeed * Time.deltaTime);
            if (color.a > 0.95 * fullColor.a) {
                color.a = 1;
                fadeIn = false;
            }
        }
        else {
            color = Color.Lerp(color, Color.clear, blinkSpeed * Time.deltaTime);
            if (color.a < 0.05 * fullColor.a) {
                color.a = 0;
                fadeIn = true && blinking;
            }
        }

        GetComponent<Image>().color = color;
    }

    public void DisableBlinking() {
        blinking = false;
    }

    public void EnableBlinking() {
        blinking = true;
    }
}
