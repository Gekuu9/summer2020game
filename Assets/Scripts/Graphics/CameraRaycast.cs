using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour {

    void Update() {
        if (Physics.Raycast(transform.position, Camera.main.transform.position - transform.position)) {
            foreach (ParticleSystem script in GetComponentsInChildren<ParticleSystem>()) {
                script.Stop();
            }
        } else {
            foreach (ParticleSystem script in GetComponentsInChildren<ParticleSystem>()) {
                if (!script.isPlaying) script.Play();
            }
        }
    }

}
