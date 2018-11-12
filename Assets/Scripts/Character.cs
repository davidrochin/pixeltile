using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public CharacterData data;

    SpriteRenderer renderer;

    private void Awake() {
        renderer = transform.GetComponentInChildren<SpriteRenderer>();
    }

    private void Start() {
        Sprite sprite = Sprite.Create(data.spriteSheet.texture, new Rect(0, 0, 32, 34), new Vector2(0.5f, 0.5f), 22.62742f);
        renderer.sprite = sprite;
    }
}
