using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelBuilder : MonoBehaviour {

    public LevelObject.TranslateParameters m_TranslateLevelParameters;

    public void BuildLevel() {
        if (!gameObject.activeSelf) return;
        IEnumerator coroutine = GetComponent<LevelObject>().LoadLevel();
        StartCoroutine(coroutine);
        EditorUtility.SetDirty(gameObject);
    }

    public void DestroyLevel() {
        if (!gameObject.activeSelf) return;
        GetComponent<LevelObject>().DestroyLevel();
        EditorUtility.SetDirty(gameObject);
    }

    public void TranslateLevel() {
        if (!gameObject.activeSelf) return;
        GetComponent<LevelObject>().TranslateLevel(m_TranslateLevelParameters);
        EditorUtility.SetDirty(gameObject);
    }
}
