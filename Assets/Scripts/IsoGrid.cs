using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoGrid : MonoBehaviour {

    public IsoPalette palette;

    public IsoGridSerializable grid;

    public bool loadTestGrid = false;

	void Awake () {
		
	}

    void Start() {
        if (loadTestGrid) {
            LoadTestGrid();
            Build();
        } 
    }

    public int CalculateSortingOrder(Vector3 pos) {
        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.y + 0.1f);
        int z = Mathf.RoundToInt(pos.z);

        Debug.Log(x + ", " + y + ", " + z);

        return - x - z + y;
    }

    public void Build() {

        float heightCorrection = 0.15f;

        for (int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                for (int z = 0; z < grid.GetLength(2); z++) {
                    GameObject tileObject = new GameObject("Tile");
                    GameObject tileSpriteObject = new GameObject("Sprite");
                    tileSpriteObject.transform.parent = tileObject.transform;

                    SpriteRenderer spriteRenderer = tileSpriteObject.AddComponent<SpriteRenderer>();
                    IsoTile isoTile = tileObject.AddComponent<IsoTile>();
                    BoxCollider boxCollider = tileObject.AddComponent<BoxCollider>();
                    spriteRenderer.sprite = palette.sprites[Random.Range(0, palette.sprites.Count - 1)];

                    // Acomodar
                    tileObject.transform.position = new Vector3(x, y - heightCorrection * y, z);

                    // Ajustar orden
                    spriteRenderer.sortingOrder = - x + y - z;

                    tileObject.transform.parent = transform;
                }
            }
        }
    }

    public void LoadTestGrid() {
        grid = new IsoGridSerializable(8, 8, 8);
        for(int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                for (int z = 0; z < grid.GetLength(2); z++) {
                    grid[x, y, z] = new IsoTileSerializable() { spriteName = "1" };
                }
            }
        }
    }
}

[System.Serializable]
public class IsoGridSerializable {

    public IsoTileSerializable[] gridArray;

    public int sizeX, sizeY, sizeZ;

    public IsoGridSerializable(int x, int y, int z) {
        sizeX = x; sizeY = y; sizeZ = z;
        gridArray = new IsoTileSerializable[x * y * z];
    }

    public IsoTileSerializable this[int x, int y, int z] {
        get {
            return gridArray[sizeX * x + sizeY * y + sizeZ * z];
        }
        set {
            gridArray[sizeX * x + sizeY * y + sizeZ * z] = value;
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
}
