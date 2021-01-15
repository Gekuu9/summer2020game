using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBox : MonoBehaviour {

    public GameObject cameraPrefab;
    public Vector3 cameraSpawnLocation;
    public Vector3 cameraSpawnRotation;
    public Vector3Int[] targetLocations;


    [HideInInspector]
    public Cinemachine.CinemachineVirtualCamera currentCamera;

    private void Start() {
        currentCamera = Instantiate(cameraPrefab, cameraSpawnLocation, Quaternion.Euler(cameraSpawnRotation)).GetComponent<Cinemachine.CinemachineVirtualCamera>();
        currentCamera.Priority = -10;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.GetComponent<Player>()) {
            currentCamera.Priority = 10;
            Trigger();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.transform.GetComponent<Player>()) {
            currentCamera.Priority = -10;
            Trigger();
        }
    }

    public void Trigger() {
        foreach (Vector3Int targetLoc in targetLocations) {
            Block block = LevelRenderer.instance.GetBlock(targetLoc);
            if (block == null || !(block is TriggerableObject)) return;
            ((TriggerableObject)block).Trigger();
        }
    }
}
