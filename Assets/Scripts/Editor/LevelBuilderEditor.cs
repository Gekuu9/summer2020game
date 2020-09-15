using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(LevelBuilder))]
public class LevelBuilderEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        LevelBuilder builder = (LevelBuilder)target;

        if (GUILayout.Button("Build Level")) {
            builder.BuildLevel();
        }

        if (GUILayout.Button("Destroy Level")) {
            builder.DestroyLevel();
        }

        if (GUILayout.Button("Translate Level")) {
            builder.TranslateLevel();
        }
    }
}
