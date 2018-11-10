using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class IsoTile : MonoBehaviour {

    public string sprite_name;

    GameObject spriteObject;

    private void Start() {
        spriteObject = transform.Find("Sprite").gameObject;
        spriteObject.transform.rotation = Camera.main.transform.rotation;
        //spriteObject.transform.localScale = Vector3.one * (Mathf.Sqrt(2));

    }

    private void Update() {
        spriteObject = transform.Find("Sprite").gameObject;
        spriteObject.transform.rotation = Camera.main.transform.rotation;
    }

}

[System.Serializable]
public class IsoTileSerializable {
    public string spriteName;
}