using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Name", menuName = "Character Data", order = 1)]
public class CharacterData : ScriptableObject {

    [Header("Information")]
    public string name;

    [Range(0.1f, 10f)]
    public float speed = 1f;

    [Header("Graphics")]
    public Sprite spriteSheet;

}