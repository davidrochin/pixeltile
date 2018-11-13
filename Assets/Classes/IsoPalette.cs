using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Isometric Tile Palette", order = 1)]
public class IsoPalette : ScriptableObject {

    public List<IsoTileData> tileData;
	
}

[System.Serializable]
public class IsoTileData {
    public Sprite sprite;
    public IsoTileCollider collider;
}

public enum IsoTileCollider {
    Cube, Floor
}