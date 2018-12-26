using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Isometric Tile Palette", order = 1)]
public class IsoPalette : ScriptableObject {

    public List<IsoTile> tiles;

    public IsoTile this[int i]{
        get {
            return tiles[i];
        }
        set {
            tiles[i] = value;
        }
    }

}

[System.Serializable]
public class IsoTile {

    public Sprite sprite;
    public Mesh mesh;
    public int offsetY = 0;

    [Range(0, 270)]
    public int rotation = 0;

}