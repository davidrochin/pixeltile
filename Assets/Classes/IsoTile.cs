using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class IsoTile : MonoBehaviour {

    public string sprite_name;

    GameObject spriteObject;

    private void Awake() {
        CorrectRotation();
    }

    public void CorrectRotation() {
        spriteObject = transform.Find("Sprite").gameObject;
        spriteObject.transform.rotation = Camera.main.transform.rotation;
    }

}

[System.Serializable]
public class IsoTileSerializable {
    public string spriteName;
    public GameObject instancedSprite;
}