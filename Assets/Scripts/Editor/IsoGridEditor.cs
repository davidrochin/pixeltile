using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IsoGrid))]
public class IsoGridEditor : Editor {

    IsoGrid isoGrid;

    bool buildMode;
    int level;
    Plane editPlane;

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

        if (!buildMode && GUILayout.Button("Build Mode")) {
            buildMode = true;
        }

        if (buildMode) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Nivel");
            level = EditorGUILayout.IntSlider(level, 1, isoGrid.grid.sizeY);
            Debug.Log(isoGrid.heightCorrection * level);
            editPlane = new Plane(Vector3.up, new Vector3(0, (level - 1) + isoGrid.heightCorrection * (level - 1), 0));
            EditorGUILayout.EndHorizontal();
        }
    }

    private void OnSceneGUI() {
        Handles.BeginGUI();

        if (GUI.Button(new Rect(5, 5, 100, 20), "Isometric View")) {
            SceneView.lastActiveSceneView.orthographic = true;
            SceneView.lastActiveSceneView.rotation = Quaternion.Euler(30f, 45f, 0f);
        }

        Handles.EndGUI();

        if (buildMode) {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            float rayHitDistance = 0f; bool onPlane = editPlane.Raycast(ray, out rayHitDistance);
            Vector3 worldPoint = ray.origin + ray.direction * rayHitDistance;
            worldPoint = new Vector3(Mathf.Round(worldPoint.x), (level - 1) + isoGrid.heightCorrection * (level - 1), Mathf.Round(worldPoint.z));

            Handles.color = Color.white;
            Handles.DrawWireCube(worldPoint, Vector3.one);
        }

        /*Handles.color = new Color(1f, 0f, 0f, 0.5f);
        Handles.DrawSolidDisc(new Vector3(0f, level, 0f), Vector3.up, 10f);
        Gizmos.DrawSphere(isoGrid.transform.position, 10f);*/
    }
}
