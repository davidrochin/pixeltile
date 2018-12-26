using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class IsoGrid : MonoBehaviour {

    public IsoPalette palette;
    public IsoGridSerializable grid;

    public bool loadTestGrid = false;
    public float heightCorrection = -0.15f;

    void Awake() {

    }

    void Start() {
        if (loadTestGrid) {
            LoadTestGrid();
            UpdateSprites();
        }
    }

    public int CalculateSortingOrder(Vector3 pos) {
        return CalculateSortingOrder(pos, 0, 0, 0);
        //return CalculateCellOrder(pos);
    }

    public int CalculateSortingOrder(Vector3 pos, int offsetX, int offsetY, int offsetZ) {

        /// Contraarrestar la posicion de la grid
        //pos -= transform.position;

        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.y);
        int z = Mathf.RoundToInt(pos.z);

        int floorAdition = grid.sizeX * grid.sizeZ;
        //return CalculateCellOrder(x + offsetX, y + offsetY, z + offsetZ);
        return CalculateCellOrder(pos + new Vector3(offsetX, offsetY, offsetZ));
    }

    private int CalculateCellOrder(int x, int y, int z) {
        return (-x * 100 + y * 100 - z * 100);
    }

    private int CalculateCellOrder(Vector3 v) {
        return (Mathf.RoundToInt(-v.x) * 100 + Mathf.RoundToInt(v.y) * 100 - Mathf.RoundToInt(v.z) * 100);
    }

    private int CalculateGridOrder() {
        List<IsoGrid> grids = new List<IsoGrid>(FindObjectsOfType<IsoGrid>());
        List<IsoGrid> ordered = new List<IsoGrid>();

        // Get the closest to the camera
        while (grids.Count > 0) {
            IsoGrid closest = grids[0];
            //grids.Remove(closest);

            foreach (IsoGrid g in grids) {
                if ((Camera.main.transform.position - g.transform.position).magnitude > (Camera.main.transform.position - closest.transform.position).magnitude) {
                    closest = g;
                }
            }

            grids.Remove(closest);
            ordered.Add(closest);
        }

        for (int i = 0; i < ordered.Count; i++) {
            if (ordered[i] == this) {
                return i;
            }
        }
        return -1;
    }

    private IsoGrid[] GetOrderedGrids() {
        List<IsoGrid> grids = new List<IsoGrid>(FindObjectsOfType<IsoGrid>());
        List<IsoGrid> ordered = new List<IsoGrid>();

        // Order from farthest to nearest
        while (grids.Count > 0) {
            IsoGrid closest = grids[0];

            foreach (IsoGrid g in grids) {
                if ((Camera.main.transform.position - g.transform.position).magnitude > (Camera.main.transform.position - closest.transform.position).magnitude) {
                    closest = g;
                }
            }

            grids.Remove(closest);
            ordered.Add(closest);
        }
        return ordered.ToArray();
    }

    public IsoCell[] GetAllCells() {
        List<IsoCell> cells = new List<IsoCell>();
        foreach (IsoCellSerializable sc in grid.gridArray) {
            if(sc.instance != null) {
                cells.Add(sc.instance);
            }
        }
        return cells.ToArray();
    }

    private int GetHighestOrder() {
        return CalculateCellOrder(grid.GetLength(0) - 1, grid.GetLength(1) - 1, grid.GetLength(2) - 1);
    }

    public void UpdateSprites() {
        for (int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                for (int z = 0; z < grid.GetLength(2); z++) {
                    UpdateSprite(x, y, z);
                }
            }
        }

        // Esconder todos los hijos del Grid
        /// Esto se hace al final, para que, si el programa truena, no dejar objectos escondidos que no sirven
        foreach(Transform t in transform) {
            t.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable;
        }
    }

    public void UpdateSprite(int x, int y, int z) {

        // Destruir la instancia actual
        if (grid[x, y, z].instance != null) {
            DestroyImmediate(grid[x, y, z].instance.gameObject);
        }

        if (grid[x, y, z].state != CellState.Empty) {
            IsoTile tile = grid[x, y, z].tile;
            GameObject cellObject = new GameObject("Tile");
            SpriteRenderer cellRenderer = new GameObject("Sprite").AddComponent<SpriteRenderer>();
            cellRenderer.transform.parent = cellObject.transform;

            IsoCell cell = cellObject.AddComponent<IsoCell>();

            // Añadir el collider
            MeshCollider collider = cellObject.AddComponent<MeshCollider>();
            collider.hideFlags = HideFlags.HideInHierarchy;
            collider.gameObject.transform.rotation = Quaternion.Euler(collider.transform.rotation.x, tile.rotation, collider.transform.rotation.z);

            if (tile.mesh != null) {
                collider.sharedMesh = tile.mesh;
            }

            // Añadir el mesh temporal (para que se puedan generar navmesh)
            MeshRenderer meshRenderer = cellObject.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = cellObject.AddComponent<MeshFilter>();
            Material material = new Material(Shader.Find("Transparent/Diffuse"));
            //Material material = new Material(Shader.Find("Diffuse"));

            meshFilter.mesh = tile.mesh;
            meshRenderer.material = material;
            cellRenderer.sprite = tile.sprite;
            material.color = new Color(0,0,0,0);

            // Acomodar
            cellObject.transform.position = new Vector3(x, y + heightCorrection * y, z) + transform.position;

            // Ajustar orden
            //cellRenderer.sortingOrder = CalculateCellOrder(x, y + tile.offsetY, z);
            cellRenderer.sortingOrder = CalculateCellOrder(cellRenderer.transform.position + Vector3.up * tile.offsetY);

            cellObject.transform.parent = transform;
            cell.CorrectRotation();

            // Registrar la instancia
            grid[x, y, z].instance = cell;
        }
    }

    public void ClearSprites() {
        while (transform.childCount != 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    public void LoadTestGrid() {
        Debug.Log("Loading Test Grid");
        grid = new IsoGridSerializable(8, 8, 8);
        for (int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                for (int z = 0; z < grid.GetLength(2); z++) {
                    grid[x, y, z] = new IsoCellSerializable() { tile = palette[0], state = CellState.Filled };
                }
            }
        }
    }

    public Int3 PointToCoord(Vector3 point) {
        float level = new Int3(point).y;
        Vector3 heightCorrected = new Vector3(point.x, point.y + heightCorrection * level, point.z) - transform.position;
        Int3 rounded = new Int3(heightCorrected);
        return rounded;
    }

    public Vector3 CoordToPoint(Int3 coord) {
        float level = coord.y;
        return new Vector3(coord.x, coord.y + heightCorrection * level, coord.z);
    }

    public Vector3 PointToGrid(Vector3 point) {
        return new Vector3(Mathf.Round(point.x), Mathf.Round(point.y) + heightCorrection * point.y, Mathf.Round(point.z));
    }

    public bool ValidateCoord(Int3 coord) {
        if (coord.x >= grid.sizeX || coord.y >= grid.sizeY || coord.z >= grid.sizeZ ||
            coord.x < 0 || coord.y < 0 || coord.z < 0) {
            return false;
        } else {
            return true;
        }
    }
}

[System.Serializable]
public class IsoGridSerializable {

    public IsoCellSerializable[] gridArray;

    public int sizeX, sizeY, sizeZ;

    public IsoGridSerializable(int x, int y, int z) {
        sizeX = x; sizeY = y; sizeZ = z;
        gridArray = new IsoCellSerializable[x * y * z];
    }

    public IsoCellSerializable this[int x, int y, int z] {
        get {
            return gridArray[x + sizeX * (y + sizeZ * z)];
        }
        set {
            gridArray[x + sizeX * (y + sizeZ * z)] = value;
        }
    }

    public int GetLength(int d) {
        switch (d) {
            case 0:
                return sizeX;
            case 1:
                return sizeY;
            case 2:
                return sizeZ;
            default:
                throw new System.Exception();
        }
    }

    public int GetSize() {
        return gridArray.Length;
    }

    public bool InBounds(int x, int y, int z) {
        if (x < sizeX && y < sizeY && z < sizeZ &&
            x >= 0 && y >= 0 && z >= 0) {
            return true;
        } else {
            return false;
        }
    }
}
