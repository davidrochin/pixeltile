using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class IsoGrid : MonoBehaviour {

    public IsoPalette palette;
    public IsoGridSerializable grid;

    public bool loadTestGrid = false;
    public float heightCorrection = -0.15f;

    void Awake () {
		
	}

    void Start() {
        if (loadTestGrid) {
            LoadTestGrid();
            UpdateSprites();
        } 
    }

    public int CalculateSortingOrder(Vector3 pos) {
        return CalculateSortingOrder(pos, 0, 0, 0);
    }

    public int CalculateSortingOrder(Vector3 pos, int offsetX, int offsetY, int offsetZ) {
        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.y);
        int z = Mathf.RoundToInt(pos.z);

        int floorAdition = grid.sizeX * grid.sizeZ;
        return CalculateSortingOrder(x + offsetX, y + offsetY, z + offsetZ);
    }

    public int CalculateSortingOrder(int x, int y, int z) {
        return - x * 100 + y * 100 - z * 100;
    }

    public void UpdateSprites() {
        for (int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                for (int z = 0; z < grid.GetLength(2); z++) {
                    UpdateSprite(x, y, z);
                }
            }
        }
    }

    public void UpdateSprite(int x, int y, int z) {

        if (grid[x, y, z].instance != null) {
            DestroyImmediate(grid[x, y, z].instance.gameObject);
        }

        if (grid[x, y, z].state != CellState.Empty) {
            GameObject cellObject = new GameObject("Tile");
            SpriteRenderer cellRenderer = new GameObject("Sprite").AddComponent<SpriteRenderer>();
            cellRenderer.transform.parent = cellObject.transform;

            IsoCell cell = cellObject.AddComponent<IsoCell>();
            BoxCollider boxCollider = cellObject.AddComponent<BoxCollider>();
            boxCollider.hideFlags = HideFlags.HideInHierarchy;
            cellRenderer.sprite = grid[x, y, z].tile.sprite;

            // Acomodar
            cellObject.transform.position = new Vector3(x, y + heightCorrection * y, z);

            // Ajustar orden
            int floorAdition = grid.sizeX * grid.sizeZ;
            cellRenderer.sortingOrder = CalculateSortingOrder(x, y, z);

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
        for(int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                for (int z = 0; z < grid.GetLength(2); z++) {
                    grid[x, y, z] = new IsoCellSerializable() { tile = palette[0], state = CellState.Filled };
                }
            }
        }
    }

    public Int3 PointToCoord(Vector3 point) {
        float level = new Int3(point).y;
        Vector3 heightCorrected = new Vector3(point.x, point.y + heightCorrection * level, point.z);
        Int3 rounded = new Int3(heightCorrected);
        return rounded;
    }

    public Vector3 CoordToPoint(Int3 coord) {
        float level = coord.y;
        return new Vector3(coord.x, coord.y + heightCorrection * level, coord.z);
    }

    public bool ValidateCoord(Int3 coord) {
        if(coord.x >= grid.sizeX || coord.y >= grid.sizeY || coord.z >= grid.sizeZ ||
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

    public bool InBounds(int x, int y, int z) {
        if (x < sizeX && y < sizeY && z < sizeZ &&
            x >= 0 && y >= 0 && z >= 0) {
            return true;
        } else {
            return false;
        }
    }
}
