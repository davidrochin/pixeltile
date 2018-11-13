using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IsoGrid))]
public class IsoGridEditor : Editor {

    IsoGrid isoGrid;

    private void OnEnable() {
        isoGrid = (IsoGrid)target;
    }

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Rebuild Sprites")) {
            isoGrid.ClearSprites();
            isoGrid.GenerateSprites();
        }
        if (GUILayout.Button("Clear Sprites")) {
            isoGrid.ClearSprites();
        }
        GUILayout.EndHorizontal();
    }

    private void OnSceneGUI() {
        Handles.BeginGUI();
        if (GUI.Button(new Rect(5, 5, 100, 20), "Isometric View")) {
            SceneView.lastActiveSceneView.orthographic = true;
            SceneView.lastActiveSceneView.rotation = Quaternion.Euler(30f, 45f, 0f);
        }
        Handles.EndGUI();
    }
}
