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

    public Sprite[] GetSprites(AnimationType animationType) {
        List<Sprite> sprites = new List<Sprite>();
        switch (animationType) {
            case AnimationType.IdleSouth:
                sprites.Add(GetSprite(0));
                break;
        }
        return sprites.ToArray();
    }

    public Sprite[] GetAnimation(int row) {
        List<Sprite> sprites = new List<Sprite>();
        int posX = 0; int posY = ((int)spriteSheet.rect.height - TILE_SIZE_Y) - TILE_SIZE_Y * row;
        int maxSprites = (int)spriteSheet.rect.width / TILE_SIZE_X;
        for (int x = 0; x < maxSprites; x++) {
            sprites.Add(Sprite.Create(spriteSheet.texture, new Rect(x * TILE_SIZE_X, posY, TILE_SIZE_X, TILE_SIZE_Y), new Vector2(0.5f, 0.5f), PPU));
        }
        return sprites.ToArray();
    }

    public Sprite GetSprite(int number) {
        int posX = 0; int posY = (int)spriteSheet.rect.height - TILE_SIZE_Y;
        return Sprite.Create(spriteSheet.texture, new Rect(posX, posY, TILE_SIZE_X, TILE_SIZE_Y), new Vector2(0.5f, 0.5f), PPU);
    }

    public const int SHEET_COUNT_X = 4;
    public const int SHEET_COUNT_Y = 6;
    public const int TILE_SIZE_X = 32;
    public const int TILE_SIZE_Y = 34;
    public const float PPU = 22.62742f;
    public enum AnimationType { IdleSouth, IdleWest, IdleEast, IdleNorth }
}