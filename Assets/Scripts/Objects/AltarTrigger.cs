using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class AltarTrigger : MultiBlockHandler, InteractableObject {

    public Transform sphere;
    public bool oneTimeTrigger;
    public Color offColor;
    public Color onColor;
    public float colorChangeSpeed;

    protected float lightMaxIntensity;

    [HideInInspector]
    public bool state;

    [HideInInspector]
    public TriggerableObject[] targets;

    [HideInInspector]
    public bool triggered = false;

    protected bool playSound;

    private bool waitingForPlayer;

    private void Start() {
        SetupMultiBlock();
        LevelRenderer.instance.player.onFindNewPath += stopWaiting;
    }

    public virtual void Setup(int stateIndex, Vector3Int[] targetPositions, Vector3Int cornerLocation) {
        state = stateIndex > 0;
        GetComponent<Animator>().SetBool("Active", state);
        Color color = state ? onColor : offColor;
        sphere.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);

        lightMaxIntensity = sphere.GetComponent<Light>().intensity;
        sphere.GetComponent<Light>().intensity = state ? lightMaxIntensity : 0;

        playSound = false;

        targets = new TriggerableObject[targetPositions.Length];
        for (int i = 0; i < targetPositions.Length; i++) {
            targets[i] = LevelRenderer.instance.GetObject(targetPositions[i] + cornerLocation).GetComponent<TriggerableObject>();
        }
    }

    public override void MovePlayerHere() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        LevelRenderer.instance.player.Move(location, 3f);
    }

    public override bool FindPathHere() {
        Vector3 location = GetComponent<BlockInfo>().gridLocation;
        return LevelRenderer.instance.player.PathMove(location, true);
    }

    public virtual void Trigger() {
        if ((!oneTimeTrigger) || (oneTimeTrigger && !triggered)) {
            triggered = true;
            state = !state;
            playSound = true;

            if (state) GetComponent<Animator>().SetTrigger("Activate");
            else GetComponent<Animator>().SetTrigger("Deactivate");

            for (int i = 0; i < targets.Length; i++) {
                targets[i].Trigger();
            }


        }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0) && ((!oneTimeTrigger) || (oneTimeTrigger && !triggered))) {
            if (Vector3.Distance(LevelRenderer.instance.player.gridPosition, GetComponent<BlockInfo>().gridLocation) <= 1) {
                LevelRenderer.instance.player.TurnPlayer(GetComponent<BlockInfo>().gridLocation - LevelRenderer.instance.player.gridPosition);
                Trigger();
                return;
            }
            if (!FindPathHere()) return;
            waitingForPlayer = true;
        }
    }

    private void Update() {
        if (sphere.GetComponent<MeshRenderer>().material.GetColor("_BaseColor") != (state ? onColor : offColor)) {
            Color color0 = sphere.GetComponent<MeshRenderer>().material.GetColor("_BaseColor");
            Color color1 = state ? onColor : offColor;
            color0 = Color.Lerp(color0, color1, colorChangeSpeed * Time.deltaTime);
            if (Mathf.Abs(color0.r + color0.g + color0.b - color1.r - color1.g - color1.b) < 0.05) color0 = color1;
            sphere.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color0);

            float intensity = Mathf.Lerp(sphere.GetComponent<Light>().intensity, state ? lightMaxIntensity : 0, colorChangeSpeed * lightMaxIntensity * Time.deltaTime);
            if (Mathf.Abs(intensity - (state ? lightMaxIntensity : 0)) < 0.1) intensity = state ? lightMaxIntensity : 0;
            sphere.GetComponent<Light>().intensity = intensity;
        } else if (playSound) {
            GetComponent<AudioSource>().Play();
            playSound = false;
        }

        if (waitingForPlayer) {
            if (Vector3.Distance(LevelRenderer.instance.player.gridPosition, GetComponent<BlockInfo>().gridLocation) <= 2 && LevelRenderer.instance.player.moveVelocity == Vector3.zero) {
                waitingForPlayer = false;
                LevelRenderer.instance.player.TurnPlayer(GetComponent<BlockInfo>().gridLocation - LevelRenderer.instance.player.gridPosition);
                Trigger();
            }
        }
    }

    private void stopWaiting() {
        waitingForPlayer = false;
    }

    private void OnMouseEnter() {
        foreach (OutlineOrthoSingle item in GetComponentsInChildren<OutlineOrthoSingle>()) {
            if ((!oneTimeTrigger) || (oneTimeTrigger && !triggered)) item.enabled = true;
        }
    }

    private void OnMouseExit() {
        foreach (OutlineOrthoSingle item in GetComponentsInChildren<OutlineOrthoSingle>()) {
            if ((!oneTimeTrigger) || (oneTimeTrigger && !triggered)) item.enabled = false;
        }
    }

    private void OnEnable() {
        foreach (OutlineOrthoSingle item in GetComponentsInChildren<OutlineOrthoSingle>()) {
            if ((!oneTimeTrigger) || (oneTimeTrigger && !triggered)) item.enabled = false;
        }
    }
}
