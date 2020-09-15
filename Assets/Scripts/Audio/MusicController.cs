using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    private void Awake() {
        instance = this;
    }

    public static MusicController instance;

    public float fadeOutTime;
    public float fadeInTime;

    [HideInInspector]
    public AudioClip currentTrack;

    private float musicVolume;

    private bool fading;

    public void SwitchTrack(AudioClip newTrack) {
        GetComponent<AudioSource>().clip = newTrack;
        currentTrack = newTrack;
        GetComponent<AudioSource>().Play();
    }

    public IEnumerator TrackTransition(AudioClip newTrack) {
        IEnumerator coroutine = FadeMusicOut();
        StartCoroutine(coroutine);
        while (GetComponent<AudioSource>().volume > 0) yield return null;

        GetComponent<AudioSource>().clip = newTrack;
        currentTrack = newTrack;

        coroutine = FadeMusicIn();
        StartCoroutine(coroutine);
    }

    public IEnumerator FadeMusicOut() {
        while (fading) yield return null;

        musicVolume = GetComponent<AudioSource>().volume;
        fading = true;

        while (GetComponent<AudioSource>().volume > 0) {
            GetComponent<AudioSource>().volume -= musicVolume * Time.deltaTime / fadeOutTime;
            yield return null;
        }

        fading = false;
    }

    public IEnumerator FadeMusicIn() {
        while (fading) yield return null;

        fading = true;

        while (GetComponent<AudioSource>().volume < musicVolume) {
            GetComponent<AudioSource>().volume += musicVolume * Time.deltaTime / fadeInTime;
            yield return null;
        }

        fading = false;
        GetComponent<AudioSource>().volume = musicVolume;
    }
}
