using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoGrid : MonoBehaviour {

    public IsoTile[,,] grid = new IsoTile[0, 0, 0];
    public Sprite tileTestSprite;

	void Awake () {
		
	}

    void Start() {
        LoadTestGrid();
        Build();
    }

    void Update () {
		
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
                    spriteRenderer.sprite = tileTestSprite;

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
