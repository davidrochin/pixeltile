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

    int selected = -1;

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

        /*if (!editMode && GUILayout.Button("Build Mode")) {
            editMode = true;
        }*/

        if (editMode) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Nivel");
            level = EditorGUILayout.IntSlider(level, 1, isoGrid.grid.sizeY);
            editPlane = new Plane(Vector3.up, new Vector3(0, (level - 1) + isoGrid.heightCorrection * (level - 1), 0));
            EditorGUILayout.EndHorizontal();

            // Grid de tiles del palette
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            List<Texture> textures = new List<Texture>();
            foreach (IsoTile t in isoGrid.palette.tiles) {
                textures.Add(AssetPreview.GetAssetPreview(t.sprite));
            }
            selected = GUILayout.SelectionGrid(selected, textures.ToArray(), (Screen.width / 50));
            selectedTile = selected != -1 ? isoGrid.palette[selected] : null;
            EditorGUILayout.EndVertical();
        }
    }

    private void OnSceneGUI() {

        IsoCell[] cells = isoGrid.GetAllCells();
        foreach (IsoCell c in cells) {
            MeshRenderer renderer = c.GetComponent<MeshRenderer>();
            if(c != null) {
                EditorUtility.SetSelectedRenderState(renderer, EditorSelectedRenderState.Hidden);
            }
        }

        Handles.BeginGUI(); // ---------------------------------------------------------------------------------------

        GUILayout.BeginArea(new Rect(5, 5, 50, 200));        

        if (GUILayout.Button(EditorGUIUtility.IconContent("ClothInspector.ViewValue"), GUILayout.Width(30))) {
            SceneView.lastActiveSceneView.orthographic = true;
            SceneView.lastActiveSceneView.rotation = Quaternion.Euler(30f, 45f, 0f);
        }

        GUILayout.Space(20);

        if (editMode) {
            mode = (EditMode)GUILayout.SelectionGrid((int)mode, new GUIContent[] {
                EditorGUIUtility.IconContent("Grid.PaintTool"),
                EditorGUIUtility.IconContent("Grid.EraserTool")}, 1, GUILayout.Width(30));
        } else if(GUILayout.Button(EditorGUIUtility.IconContent("Terrain Icon"), GUILayout.Width(30), GUILayout.Height(30))) {
            editMode = true;
            Repaint();
            selected = 0;
        }
        GUILayout.EndArea();

        if (editMode) {
            level = (int)GUI.VerticalSlider(new Rect(Screen.width - 30, Screen.height - 250, 20, 200), level, isoGrid.grid.sizeY, 1);
            editPlane = new Plane(Vector3.up, new Vector3(0, (level - 1) + isoGrid.heightCorrection * (level - 1), 0));
        }

        Handles.EndGUI(); // -----------------------------------------------------------------------------------------

        if (editMode) {

            // Obtener la coordenada en la grid a partir de la posicion del mouse
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            float rayHitDistance = 0f; bool onPlane = editPlane.Raycast(ray, out rayHitDistance);
            Vector3 worldPoint = ray.origin + ray.direction * rayHitDistance;
            Int3 coord = isoGrid.PointToCoord(worldPoint);

            //Vector3 markerPos = isoGrid.CoordToPoint(coord);
            Vector3 markerPos = isoGrid.PointToGrid(worldPoint);

            // Revisar si la coordenada es válida
            if (isoGrid.ValidateCoord(coord)) {
                Handles.color = Color.white;
            } else {
                Handles.color = Color.gray;
            }

            // Dibujar un cubo en el punto
            Handles.DrawWireCube(markerPos, Vector3.one);
            Handles.DrawDottedLine(markerPos + Vector3.down * 0.5f, markerPos + Vector3.down * 0.5f + Vector3.down * markerPos.y, 1f);
            Handles.DrawWireCube(markerPos + Vector3.down * markerPos.y + Vector3.down * 0.5f, new Vector3(1f, 0f, 1f));

            // Para no perder el focus
            if (editMode && isoGrid.ValidateCoord(coord)) {
                Selection.activeObject = target;
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            }

            /// Obtener el evento actual
            Event currentEvent = Event.current;

            // Edicion
            if ((currentEvent.type == EventType.MouseDown || currentEvent.type == EventType.MouseDrag) && currentEvent.button == 0) {
                switch (mode) {
                    case EditMode.Paint: {
                            if (isoGrid.grid.InBounds(coord.x, coord.y, coord.z)) {
                                isoGrid.grid[coord.x, coord.y, coord.z].state = CellState.Filled;
                                isoGrid.grid[coord.x, coord.y, coord.z].tile = selectedTile;
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

            // Subir o bajar de capa con la ruedita del mouse si se está presionando CTRL
            if(currentEvent.control && currentEvent.isScrollWheel) {
                int delta = -(int)(currentEvent.delta.y / 3.0f);
                level = Mathf.Clamp(level + delta, 1, isoGrid.grid.sizeY);

                if (isoGrid.ValidateCoord(coord)) {
                    SceneView.currentDrawingSceneView.pivot += Vector3.up * delta;
                }
                
                currentEvent.Use();
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
