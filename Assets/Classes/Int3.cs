using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Int3 {

    public int x, y, z;

    public Int3(Vector3 vector) : this(){
        x = Mathf.RoundToInt(vector.x);
        y = Mathf.RoundToInt(vector.y);
        z = Mathf.RoundToInt(vector.z);
    }

    public Int3(int x, int y, int z) {
        this.x = x; this.y = y; this.z = z;
    }

    public override string ToString() {
        return "(" + x + ", " + y + ", " + z + ")";
    }
}
