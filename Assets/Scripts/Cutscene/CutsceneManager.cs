using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Cinemachine;
using UnityEngine;
using UnityEngine.Audio;

public class CutsceneManager : MonoBehaviour {

    public static CutsceneManager instance;

    private void Start() {
        instance = this;
    }

    private GameObject currentCamera;
    private bool fadeMusic;
    private bool suspendInput;

    public void PlaySimpleCutscene(GameObject camera, float length, bool fadeMusic, bool suspendInput) {
        if (suspendInput) EventHandler.instance.SuppressInput();
        this.suspendInput = suspendInput;
        if (currentCamera != null) Destroy(currentCamera);
        currentCamera = Instantiate(camera, transform);
        this.fadeMusic = fadeMusic;
        if (fadeMusic) MusicController.instance.transform.GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.FindSnapshot("FadeBackground").TransitionTo(1);
        IEnumerator coroutine = WaitForSeconds(length);
        StartCoroutine(coroutine);
    }

    public void EndCutscene() {
        currentCamera.GetComponent<CinemachineVirtualCamera>().Priority = -1;
        if (fadeMusic) MusicController.instance.transform.GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.FindSnapshot("Default").TransitionTo(1);
        if (suspendInput) EventHandler.instance.HandleInput();
    }

    public IEnumerator WaitForSeconds(float time) {
        yield return new WaitForSeconds(time);
        EndCutscene();
    }
}
