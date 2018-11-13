using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public CharacterData data;

    SpriteRenderer renderer;
    CharacterMotor motor;

    Coroutine animationCoroutine;

    private CardinalDirection currentDirection;

    private void Awake() {
        renderer = transform.GetComponentInChildren<SpriteRenderer>();
        motor = GetComponent<CharacterMotor>();
    }

    private void Start() {
        currentDirection = DetermineDirection();
        PlayAnimation(data.graphics.GetAnimation(AnimationType.Running, DetermineDirection()), 0.1f);
    }

    private void Update() {
        if(DetermineDirection() != currentDirection) {
            PlayAnimation(data.graphics.GetAnimation(AnimationType.Running, DetermineDirection()), 0.1f);
            currentDirection = DetermineDirection();
        }
    }

    public void PlayAnimation(Sprite[] animation, float frequency) {
        if(animationCoroutine != null) {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimationCoroutine(animation, frequency));
    }

    public CardinalDirection DetermineDirection() {
        float r = motor.transform.rotation.eulerAngles.y;
        Debug.Log(r);
        if((r > 337.5 && r <= 361) || (r >= 0f && r <= 22.5)) {
            return CardinalDirection.NW;
        } else if (r > 22.5 && r <= 67.5) {
            return CardinalDirection.N;
        } else if (r > 67.5 && r <= 112.5) {
            return CardinalDirection.NE;
        } else if (r > 112.5 && r <= 157.5) {
            return CardinalDirection.E;
        } else if (r > 157.5 && r <= 202.5) {
            return CardinalDirection.SE;
        } else if (r > 202.5 && r <= 247.5) {
            return CardinalDirection.S;
        } else if (r > 247.5 && r <= 292.5) {
            return CardinalDirection.SW;
        } else if (r > 292.5 && r <= 337.5) {
            return CardinalDirection.W;
        }
        return CardinalDirection.S;
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

public enum CardinalDirection {
    S, SW, W, NW, N, NE, E, SE
}