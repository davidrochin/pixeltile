using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoGrid : MonoBehaviour {

    public IsoTile[,,] grid = new IsoTile[0, 0, 0];
    public IsoPalette palette;

    public bool loadTestGrid = false;

	void Awake () {
		
	}

    void Start() {
        if (loadTestGrid) {
            LoadTestGrid();
            Build();
        } 
    }

    void Update () {
		
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
                    spriteRenderer.sortingOrder = -x + y - z;

                    tileObject.transform.parent = transform;
                }
            }
        }
    }

    public void LoadTestGrid() {
        grid = new IsoTile[8, 8, 8];
        for(int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                for (int z = 0; z < grid.GetLength(2); z++) {
                    grid[x, y, z] = new IsoTile() { sprite_name = "1" };
                }
            }
        }
    }
}
