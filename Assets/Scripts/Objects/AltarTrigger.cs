using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class AltarTrigger : Block, InteractableObject {

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

    private List<Vector3Int> gridLocations;

    private void Start() {
        LevelRenderer.instance.player.onFindNewPath += stopWaiting;

        gridLocations = new List<Vector3Int>();
        for (int x = gridLocation.x + bottomCorner.x; x < gridLocation.x + topCorner.x; x++) {
            for (int y = gridLocation.y + bottomCorner.y; y < gridLocation.y + topCorner.y; y++) {
                for (int z = gridLocation.z + bottomCorner.z; z < gridLocation.z + topCorner.z; z++) {
                    gridLocations.Add(new Vector3Int(x, y, z));
                }
            }
        }
    }

    private void OnEnable() {
        GetComponent<Animator>().SetBool("Active", state);
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
            targets[i] = LevelRenderer.instance.GetBlock(targetPositions[i] + cornerLocation).GetComponent<TriggerableObject>();
        }
    }

    public Vector3Int FindClosestLocation(Vector3Int pos) {
        if (gridLocations == null) return gridLocation;
        Vector3Int minLoc = bottomCorner + gridLocation;
        foreach (Vector3Int loc in gridLocations) {
            if (Vector3Int.Distance(minLoc, pos) > Vector3Int.Distance(loc, pos)) {
                minLoc = loc;
            }
        }
        return minLoc;
    }

    public Vector3Int FindClosestLocation() {
        if (gridLocations == null) return gridLocation;
        Vector3Int minLoc = bottomCorner + gridLocation;
        Vector3Int pos = Vector3Int.RoundToInt(LevelRenderer.instance.player.gridPosition);
        foreach (Vector3Int loc in gridLocations) {
            if (Vector3Int.Distance(minLoc, pos) > Vector3Int.Distance(loc, pos)) {
                minLoc = loc;
            }
        }
        return minLoc;
    }

    public override bool MovePlayerHere() {
        LevelRenderer.instance.player.Move(FindClosestLocation(), 3f);
        return true;
    }

    public override bool FindPathHere() {
        return LevelRenderer.instance.player.PathMove(FindClosestLocation(), true);
    }

    public virtual void Trigger() {
        if ((!oneTimeTrigger) || (oneTimeTrigger && !triggered)) {
            triggered = true;
            state = !state;
            playSound = true;
            if (oneTimeTrigger) GetComponent<MeshOutline>().enabled = false;

            if (state) GetComponent<Animator>().SetTrigger("Activate");
            else GetComponent<Animator>().SetTrigger("Deactivate");

            for (int i = 0; i < targets.Length; i++) {
                targets[i].Trigger();
            }


        }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0) && ((!oneTimeTrigger) || (oneTimeTrigger && !triggered))) {
            if (Vector3.Distance(LevelRenderer.instance.player.gridPosition, FindClosestLocation()) <= 1) {
                LevelRenderer.instance.player.TurnPlayer(FindClosestLocation() - LevelRenderer.instance.player.gridPosition);
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
            if (Vector3.Distance(LevelRenderer.instance.player.gridPosition, FindClosestLocation()) < 2 && LevelRenderer.instance.player.moveVelocity == Vector3.zero) {
                waitingForPlayer = false;
                LevelRenderer.instance.player.TurnPlayer(FindClosestLocation() - LevelRenderer.instance.player.gridPosition);
                Trigger();
            }
        }
    }

    private void stopWaiting() {
        waitingForPlayer = false;
    }
}
