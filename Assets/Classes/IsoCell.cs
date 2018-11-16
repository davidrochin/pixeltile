using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class IsoCell : MonoBehaviour {
    public IsoTile tile;
    public CellState state;

    private void Awake() {
        CorrectRotation();
    }

    public void CorrectRotation() {
        GameObject sprite = transform.Find("Sprite").gameObject;
        sprite.transform.rotation = Camera.main.transform.rotation;
    }

}

[System.Serializable]
public class IsoCellSerializable {
    public IsoTile tile;
    public CellState state;
    public IsoCell instance;
}

public enum CellState { Empty, Filled }