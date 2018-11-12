using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public CharacterData data;

    SpriteRenderer renderer;
    CharacterMotor motor;

    Coroutine animationCoroutine;

    private void Awake() {
        renderer = transform.GetComponentInChildren<SpriteRenderer>();
    }

    private void Start() {
        PlayAnimation(data.graphics.GetAnimation(0), 0.5f);
    }

    public void PlayAnimation(Sprite[] animation, float frequency) {
        if(animationCoroutine != null) {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimationCoroutine(animation, frequency));
    }

    private IEnumerator AnimationCoroutine(Sprite[] animation, float frequency) {
        int index = 0;
        while (true) {
            if(index >= animation.Length) {
                index = 0;
            }
            renderer.sprite = animation[index];
            index++;
            yield return new WaitForSeconds(frequency);
        }
    }
}
