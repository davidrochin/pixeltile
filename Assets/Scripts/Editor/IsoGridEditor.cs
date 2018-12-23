using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(IsoGrid))]
public class IsoGridEditor : Editor {

    IsoGrid isoGrid;

    bool editMode;
    int level;
    EditMode mode;
    Plane editPlane;
    IsoTile selectedTile;

    private void OnEnable() {
        isoGrid = (IsoGrid)target;
    }

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Rebuild Sprites")) {
            isoGrid.ClearSprites();
            isoGrid.UpdateSprites();
        }
        if (GUILayout.Button("Clear Sprites")) {
            isoGrid.ClearSprites();
        }
        if (GUILayout.Button("Fill")) {
            isoGrid.LoadTestGrid();
        }
        GUILayout.EndHorizontal();

        if (!editMode && GUILayout.Button("Build Mode")) {
            editMode = true;
        }

        if (editMode) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Nivel");
            level = EditorGUILayout.IntSlider(level, 1, isoGrid.grid.sizeY);
            editPlane = new Plane(Vector3.up, new Vector3(0, (level - 1) + isoGrid.heightCorrection * (level - 1), 0));
            EditorGUILayout.EndHorizontal();
        }
    }

    private void OnSceneGUI() {

        Handles.BeginGUI(); // ---------------------------------------------------------------------------------------

        GUILayout.BeginArea(new Rect(5, 5, 50, 200));        

        if (GUILayout.Button(EditorGUIUtility.IconContent("SceneViewOrtho"), GUILayout.Width(30))) {
            SceneView.lastActiveSceneView.orthographic = true;
            SceneView.lastActiveSceneView.rotation = Quaternion.Euler(30f, 45f, 0f);
        }

        GUILayout.Space(20);

        if (editMode) {
            mode = (EditMode)GUILayout.SelectionGrid((int)mode, new GUIContent[] {
                EditorGUIUtility.IconContent("Grid.PaintTool"),
                EditorGUIUtility.IconContent("Grid.EraserTool")}, 1, GUILayout.Width(30));
        }
        GUILayout.EndArea();


        level = (int)GUI.VerticalSlider(new Rect(Screen.width - 30, Screen.height - 250, 20, 200), level, isoGrid.grid.sizeY, 1);
        editPlane = new Plane(Vector3.up, new Vector3(0, (level - 1) + isoGrid.heightCorrection * (level - 1), 0));

        Handles.EndGUI(); // -----------------------------------------------------------------------------------------

        if (editMode) {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            float rayHitDistance = 0f; bool onPlane = editPlane.Raycast(ray, out rayHitDistance);
            Vector3 worldPoint = ray.origin + ray.direction * rayHitDistance;
            Int3 coord = isoGrid.PointToCoord(worldPoint);

            if (isoGrid.ValidateCoord(coord)) {
                Handles.color = Color.white;
            } else {
                Handles.color = Color.gray;
            }

            Vector3 markerPos = isoGrid.CoordToPoint(coord);
            Handles.DrawWireCube(markerPos, Vector3.one);

            // Dibujar el Sprite
            //Handles.

            // Para no perder el focus
            if(editMode) {
                Selection.activeObject = target;
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            }

            // Edicion
            Event currentEvent = Event.current;
            if ((currentEvent.type == EventType.MouseDown || currentEvent.type == EventType.MouseDrag) && currentEvent.button == 0) {
                switch (mode) {
                    case EditMode.Paint: {
                            if (isoGrid.grid.InBounds(coord.x, coord.y, coord.z)) {
                                isoGrid.grid[coord.x, coord.y, coord.z].state = CellState.Filled;
                                isoGrid.grid[coord.x, coord.y, coord.z].tile = isoGrid.palette[0];
                                isoGrid.UpdateSprite(coord.x, coord.y, coord.z);
                                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                            }
                            break;
                        }
                    case EditMode.Erase: {
                            if (isoGrid.grid.InBounds(coord.x, coord.y, coord.z)) {
                                isoGrid.grid[coord.x, coord.y, coord.z].state = CellState.Empty;
                                isoGrid.UpdateSprite(coord.x, coord.y, coord.z);
                                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                            }
                            break;
                        }
                    case EditMode.Replace: {
                            break;
                        }
                }
                
            }
        }

        // Actualizar la interfaz repetidamente
        SceneView.RepaintAll();
        //if (Event.current.type == EventType.MouseMove) SceneView.RepaintAll();
    }

    private bool IsValidInGrid(int x, int y, int z) {
        return true;
    }

    public enum EditMode { Paint, Erase, Replace }
}
