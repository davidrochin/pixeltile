using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IsoPalette))]
public class PaletteEditor : Editor {

    IsoPalette palette;
    int selected;

    GUIStyle sectionHeaderStyle = new GUIStyle();
    GUIStyle gridFrameStyle = new GUIStyle();

    int iconWidth = 0;

    private void OnEnable() {
        palette = (IsoPalette)target;
        selected = 0;

        InitializeStyles();

        iconWidth = AssetPreview.GetAssetPreview(palette[0].sprite).height;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GUILayout.Space(50);
        GUILayout.Label("Selected", sectionHeaderStyle);

        // Draw selected tile properties
        IsoTile tile = palette.tiles[selected];
        if (tile != null) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("tiles").GetArrayElementAtIndex(selected).FindPropertyRelative("sprite"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("tiles").GetArrayElementAtIndex(selected).FindPropertyRelative("mesh"));
        }

        GUILayout.Space(20);

        // Draw tile grid controls
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", GUILayout.Width(40))) {
            palette.tiles.Add(new IsoTile());
        }
        GUILayout.Button("-", GUILayout.Width(40));
        GUILayout.EndHorizontal();

        // Draw tile grid
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        List<Texture> textures = new List<Texture>();
        foreach (IsoTile t in palette.tiles) {
            textures.Add(AssetPreview.GetAssetPreview(t.sprite));
        }
        selected = GUILayout.SelectionGrid(selected, textures.ToArray(), (Screen.width / 50));
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    private void InitializeStyles() {
        sectionHeaderStyle.fontStyle = FontStyle.Bold;
    }
}
