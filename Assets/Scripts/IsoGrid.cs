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
        for (int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                for (int z = 0; z < grid.GetLength(2); z++) {
                    GameObject tileObject = new GameObject();
                    SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = tileTestSprite;
                    tileObject.transform.position += (Vector3) new Vector2(x * 0.5f, -x * 0.25f);
                }
            }
        }
    }

    public void LoadTestGrid() {
        grid = new IsoTile[8, 8, 8];
        for(int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                for (int z = 0; z < grid.GetLength(2); z++) {
                    grid[x, y, z] = new IsoTile() { sprite_name = "1", collisionType = IsoTile.CollisionType.Block };
                }
            }
        }
    }
}
