using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoTile {

    public string sprite_name;
    public CollisionType collisionType;

    public enum CollisionType { None, Block, Small }
}
