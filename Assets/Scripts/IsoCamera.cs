using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCamera : MonoBehaviour {

    public Transform target;
    public float angle = 30f;
    public float distance = 20f;

    CharacterMotor character;

    private void Start() {
        character = target.GetComponent<CharacterMotor>();
        if(character != null) {
            character.OnFrameFinish += Follow;
        }
    }

    void Follow() {
        transform.rotation = Quaternion.Euler(30f, 45f, 0f);
        transform.position = target.position - transform.forward * distance;
    }
}
