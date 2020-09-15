using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    public AudioSource music;
    public Image screenFadeMask;

    public float fadeOutTime;

    private IEnumerator Start() {
        yield return new WaitForSeconds(3);

        music.Play();
    }

    public IEnumerator FadeOut() {
        while (music.volume > 0) {
            Color color = screenFadeMask.color;
            color.a += Time.deltaTime / fadeOutTime;
            screenFadeMask.color = color;
            music.volume -= 0.5f * Time.deltaTime / fadeOutTime;
            yield return null;
        }

        SceneManager.LoadScene("Level 1");
    }

    public void OnClickNewGame() {
        IEnumerator coroutine = FadeOut();
        StartCoroutine(coroutine);
    }
}
