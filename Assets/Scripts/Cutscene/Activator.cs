using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour {

    public ActivationInfo[] objects;

    // Start is called before the first frame update
    void Start() {
        foreach (ActivationInfo info in objects) {
            IEnumerator coroutine = ActivateObject(info);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator ActivateObject(ActivationInfo info) {
        yield return new WaitForSeconds(info.delay);
        LevelRenderer.instance.GetBlock(info.location).GetComponent<TriggerableObject>().Trigger();
    }

    [Serializable]
    public struct ActivationInfo {
        public Vector3Int location;
        public float delay;
    }
}
