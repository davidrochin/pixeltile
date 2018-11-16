using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Isometric Tile Palette", order = 1)]
public class IsoPalette : ScriptableObject {

    public List<IsoTile> tileData;

    public IsoTile this[int i]{
        get {
            return tileData[i];
        }
        set {
            tileData[i] = value;
        }
    }

}

[System.Serializable]
public class IsoTile {
    public Sprite sprite;
    public IsoTileCollider collider;
}

public enum IsoTileCollider {
    Cube, Floor
}