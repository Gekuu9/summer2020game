using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float zoomSensitivity;
    public float zoomSpeed;
    public float zoomMin;
    public float zoomMax;

    public float rotateSpeed;
    public float softZoneWidth;
    public float rotationAmount;

    private float zoom;

    private float rotation = 0;
    private int waitFrames;

    private void Start() {
        zoom = GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize;
    }
    private void Update() {
        zoom -= Input.mouseScrollDelta.y * zoomSensitivity;
        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);

        if (Input.GetKey(KeyCode.LeftArrow) && rotation == 0) {
            rotation = rotationAmount;
            GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_SoftZoneWidth = 0;
            waitFrames = -1;
        } else if (Input.GetKey(KeyCode.RightArrow) && rotation == 0) {
            rotation = -rotationAmount;
            GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_SoftZoneWidth = 0;
            waitFrames = -1;
        }
    }

    private void LateUpdate() {
        GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = Mathf.Lerp(GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize, zoom, Time.deltaTime * zoomSpeed);

        if (waitFrames == 0 && rotation == 0) {
            GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_SoftZoneWidth = softZoneWidth;
            waitFrames = -1;
        } else if (waitFrames > 0) {
            waitFrames--;
        }

        if (rotation > 0) {
            if (rotation - rotateSpeed * Time.deltaTime <= 0.1) {
                transform.RotateAround(LevelRenderer.instance.player.transform.position, Vector3.up, rotation);
                rotation = 0;
                waitFrames = 2;
            } else {
                transform.RotateAround(LevelRenderer.instance.player.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
                rotation -= rotateSpeed * Time.deltaTime;
                waitFrames = -1;
            }
        } else if (rotation < 0) {
            if ((rotation + rotateSpeed * Time.deltaTime) >= -0.1) {
                transform.RotateAround(LevelRenderer.instance.player.transform.position, Vector3.up, rotation);
                rotation = 0;
                waitFrames = 2;
            } else {
                transform.RotateAround(LevelRenderer.instance.player.transform.position, Vector3.up, -rotateSpeed * Time.deltaTime);
                rotation += rotateSpeed * Time.deltaTime;
                waitFrames = -1;
            }
        }
        
    }
}
