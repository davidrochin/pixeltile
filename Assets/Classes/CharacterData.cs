using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Name", menuName = "Character Data", order = 1)]
public class CharacterData : ScriptableObject {

    [Header("Information")]
    public string name;

    [Range(0.1f, 10f)]
    public float speed = 1f;

    public CharacterGraphics graphics;

}

[System.Serializable]
public class CharacterGraphics {
    public Sprite spriteSheet;
    public float shadowSize = 0.5f;
    public float pivotHeight = 0.2352f;

    public Sprite[] GetAnimation(Animation animation) {
        List<Sprite> sprites = new List<Sprite>();
        int animationNumber = (int)animation;
        return GetAnimation(animationNumber);
    }

    public Sprite[] GetAnimation(AnimationType type, CardinalDirection direction) {
        if(type == AnimationType.Running) {
            switch (direction) {
                case CardinalDirection.S:
                    return GetAnimation(Animation.RunningS);
                case CardinalDirection.SW:
                    //return GetAnimation(Animation.RunningSW);
                    return GetAnimation(Animation.RunningS);
                case CardinalDirection.W:
                    return GetAnimation(Animation.RunningW);
                case CardinalDirection.NW:
                    //return GetAnimation(Animation.RunningNW);
                    return GetAnimation(Animation.RunningN);
                case CardinalDirection.N:
                    return GetAnimation(Animation.RunningN);
                case CardinalDirection.NE:
                    //return GetAnimation(Animation.RunningNE);
                    return GetAnimation(Animation.RunningN);
                case CardinalDirection.E:
                    return GetAnimation(Animation.RunningE);
                case CardinalDirection.SE:
                    //return GetAnimation(Animation.RunningSE);
                    return GetAnimation(Animation.RunningS);
            }
        } else if (type == AnimationType.Idle) {
            switch (direction) {
                case CardinalDirection.S:
                    return GetAnimation(Animation.IdleS);
                case CardinalDirection.SW:
                    //return GetAnimation(Animation.IdleSW);
                    return GetAnimation(Animation.IdleS);
                case CardinalDirection.W:
                    return GetAnimation(Animation.IdleW);
                case CardinalDirection.NW:
                    //return GetAnimation(Animation.IdleNW);
                    return GetAnimation(Animation.IdleN);
                case CardinalDirection.N:
                    return GetAnimation(Animation.IdleN);
                case CardinalDirection.NE:
                    //return GetAnimation(Animation.IdleNE);
                    return GetAnimation(Animation.IdleN);
                case CardinalDirection.E:
                    return GetAnimation(Animation.IdleE);
                case CardinalDirection.SE:
                    //return GetAnimation(Animation.IdleSE);
                    return GetAnimation(Animation.IdleS);
            }
        }
        return GetAnimation(Animation.IdleS);
    }

    public Sprite[] GetAnimation(int row) {
        List<Sprite> sprites = new List<Sprite>();
        int posX = 0; int posY = ((int)spriteSheet.rect.height - TILE_SIZE_Y) - TILE_SIZE_Y * row;
        int maxSprites = (int)spriteSheet.rect.width / TILE_SIZE_X;
        for (int x = 0; x < maxSprites; x++) {
            sprites.Add(Sprite.Create(spriteSheet.texture, new Rect(x * TILE_SIZE_X, posY, TILE_SIZE_X, TILE_SIZE_Y), new Vector2(0.5f, pivotHeight), PPU));
        }
        return sprites.ToArray();
    }

    public Sprite GetSprite(int number) {
        int posX = 0; int posY = (int)spriteSheet.rect.height - TILE_SIZE_Y;
        return Sprite.Create(spriteSheet.texture, new Rect(posX, posY, TILE_SIZE_X, TILE_SIZE_Y), new Vector2(0.5f, pivotHeight), PPU);
    }

    public const int SHEET_COUNT_X = 4;
    public const int SHEET_COUNT_Y = 6;
    public const int TILE_SIZE_X = 32;
    public const int TILE_SIZE_Y = 34;
    public const float PPU = 22.62742f;
}

public enum AnimationType {
    Idle, Running
}

public enum Animation {
    IdleS, IdleSW, IdleW, IdleNW, IdleN, IdleNE, IdleE, IdleSE,
    RunningS, RunningSW, RunningW, RunningNW, RunningN, RunningNE, RunningE, RunningSE
}