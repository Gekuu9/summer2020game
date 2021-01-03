using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LevelRenderer : MonoBehaviour {

    // Singleton self-reference
    public static LevelRenderer instance;

    [Tooltip ("Player script attached to player gameobject")]
    public Player player;

    [Tooltip ("Prefab of the first level to be loaded")]
    public GameObject levelPrefab;

    [Tooltip ("MainUI script on canvas in scene")]
    public MainUI mainUICanvas;

    // Prefab of the currently active level
    [HideInInspector]
    public GameObject currentLevelPrefab;

    // LevelObject script attached to currently active level instance
    [HideInInspector]
    public LevelObject levelObject;

    // Whether the music will change when transitioning the level
    private bool changeMusic;

    private void LoadFirstLevel() {
        GameObject levelObjectInstance = Instantiate(levelPrefab, transform);
        levelObject = levelObjectInstance.GetComponent<LevelObject>();
        currentLevelPrefab = levelPrefab;
        levelObject.gameObject.SetActive(true);

        MusicController.instance.SwitchTrack(levelObject.music);

        IEnumerator coroutine = LoadAdjacentLevels();
        StartCoroutine(coroutine);

        coroutine = mainUICanvas.LevelFadeInLinear(3f);
        StartCoroutine(coroutine);
    }

    private void Awake() {
        instance = this;

        LoadFirstLevel();
    }

    // Instantiate each level that is adjacent to the active level
    private IEnumerator LoadAdjacentLevels() {
        // If levelObject's adjacent levels array doesn't exist or is too short, create a new one
        if (levelObject.adjacentLevels == null || levelObject.adjacentLevels.Length != levelObject.levelTransitionInfo.Length)
            levelObject.adjacentLevels = new GameObject[levelObject.levelTransitionInfo.Length];

        // Instantiate each adjacent level and set them to inactive
        for (int i = 0; i < levelObject.levelTransitionInfo.Length; i++) {
            LevelObject.LevelTransitionInfo lvl = levelObject.levelTransitionInfo[i];

            // If level at index i has already been instantiated, continue and don't instantiate a new copy
            if (levelObject.adjacentLevels[i] != null) continue;

            // Check if level at index i has already been instantiated at a different index (i.e. different entrance to same level)
            // If so, continue and don't instantiate a new copy
            bool cont = false;
            for (int j = 0; j < levelObject.levelTransitionInfo.Length; j++) {
                if (levelObject.levelTransitionInfo[j].level == lvl.level && i != j) {
                    if (levelObject.adjacentLevels[j] != null) {
                        levelObject.adjacentLevels[i] = levelObject.adjacentLevels[j];
                        cont = true;
                        break;
                    }
                }
            }
            if (cont) continue;

            levelObject.adjacentLevels[i] = Instantiate(lvl.level, transform);
            levelObject.adjacentLevels[i].SetActive(false);
            yield return null;
        }

        CheckLevelAdjacency();
    }

    // For each adjacent level, check if it is also adjacent to any of the active level's other adjacent levels
    // If so, add them to that level's adjacent list
    private void CheckLevelAdjacency() {
        for (int i = 0; i < levelObject.adjacentLevels.Length; i++) {
            LevelObject adjLevel = levelObject.adjacentLevels[i].GetComponent<LevelObject>();

            // If levelObject's adjacent levels array doesn't exist or is too short, create a new one
            if (adjLevel.adjacentLevels == null || adjLevel.adjacentLevels.Length != adjLevel.levelTransitionInfo.Length)
                adjLevel.adjacentLevels = new GameObject[adjLevel.levelTransitionInfo.Length];

            // Check each of the active level's adjacent levels to see if they match any from the level with index i
            for (int j = 0; j < levelObject.levelTransitionInfo.Length; j++) {
                for (int k = 0; k < adjLevel.levelTransitionInfo.Length; k++) {
                    if (levelObject.levelTransitionInfo[j].level == adjLevel.levelTransitionInfo[k].level) {
                        adjLevel.adjacentLevels[k] = levelObject.adjacentLevels[j];
                        break;
                    }
                }
            }

            // Check if the level at index i has the active level listed as adjacent
            for (int n = 0; n < adjLevel.levelTransitionInfo.Length; n++) {
                if (currentLevelPrefab == adjLevel.levelTransitionInfo[n].level) {
                    adjLevel.adjacentLevels[n] = levelObject.gameObject;
                    break;
                }
            }
        }
    }
    
    private IEnumerator LevelFade(int levelIndex) {
        // If the screen is already fading in or out, wait until it's done to fade again
        while (mainUICanvas.status != MainUI.FadeStatus.clear) yield return null;

        IEnumerator coroutine = mainUICanvas.LevelFadeOut();
        StartCoroutine(coroutine);

        changeMusic = levelObject.adjacentLevels[levelIndex].GetComponent<LevelObject>().music != levelObject.music;

        if (changeMusic) {
            coroutine = MusicController.instance.FadeMusicOut();
            StartCoroutine(coroutine);
        }

        // Wait for the level to fade out completely
        while (mainUICanvas.status != MainUI.FadeStatus.black) yield return null;

        // Load next level, then let one frame pass to allow start functions to run
        LoadNextLevel(levelIndex);
        if (changeMusic) {
            MusicController.instance.SwitchTrack(levelObject.music);
        }
        yield return null;

        coroutine = mainUICanvas.LevelFadeIn();
        StartCoroutine(coroutine);

        if (changeMusic) {
            coroutine = MusicController.instance.FadeMusicIn();
            StartCoroutine(coroutine);
        }

        coroutine = LoadAdjacentLevels();
        StartCoroutine(coroutine);
    }

    private void LoadNextLevel(int levelIndex) {
        // Get info about next level
        currentLevelPrefab = levelObject.levelTransitionInfo[levelIndex].level;
        Vector3 playerSpawnLocation = levelObject.levelTransitionInfo[levelIndex].playerSpawnLocation;
        Quaternion playerSpawnRotation = levelObject.levelTransitionInfo[levelIndex].playerSpawnRotation;

        // Switch the active level to the new one
        levelObject.gameObject.SetActive(false);
        levelObject = levelObject.adjacentLevels[levelIndex].GetComponent<LevelObject>();
        levelObject.gameObject.SetActive(true);

        // Move player to correct position
        player.ResetPlayerAtPosition(playerSpawnLocation, playerSpawnRotation);
    }

    public void LevelTransition(int levelIndex) {
        IEnumerator coroutine = LevelFade(levelIndex);
        StartCoroutine(coroutine);
    }

    // Get the block gameobject at the specified location on the active level grid
    public Block GetBlock(Vector3Int location) {
        return levelObject.GetBlock(location);
    }

    public void MoveBlock(Vector3Int oldPos, Vector3Int newPos) {
        levelObject.MoveBlock(oldPos, newPos);
    }

    public void SwapBlocks(Vector3Int location1, Vector3Int location2) {
        levelObject.SwapObjects(location1, location2);
    }
}
