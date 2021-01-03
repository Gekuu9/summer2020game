using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OrbMirror))]
public class InputTriggerTest : Editor {

    public int input;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        OrbMirror obj = (OrbMirror)target;

        if (GUILayout.Button("Trigger Input")) {
            obj.Trigger();
        }
    }
}
