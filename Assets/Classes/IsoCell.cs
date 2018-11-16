using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class IsoCell : MonoBehaviour {

    public CellState state;
    public string spriteName;

    private void Awake() {
        CorrectRotation();
    }

    public void CorrectRotation() {
        GameObject sprite = transform.Find("Sprite").gameObject;
        sprite.transform.rotation = Camera.main.transform.rotation;
    }

}

[System.Serializable]
public class IsoTileSerializable {
    public CellState state;
    public string spriteName;
    public IsoCell instance;
}

public enum CellState { Empty, Filled }