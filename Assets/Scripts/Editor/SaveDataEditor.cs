using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveDataManager))]
public class SaveDataEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        SaveDataManager save = (SaveDataManager)target;

        if (GUILayout.Button("Save Data")) {
            save.SaveFile();
        }

        if (GUILayout.Button("Load Data")) {
            save.LoadFile();
        }

        if (GUILayout.Button("Reset Save Data")) {
            save.ResetSaveFile();
        }
    }
}
