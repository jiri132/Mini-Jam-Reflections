using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControls : MovementManager
{
    public override Vector2 Direction()
    {
        int up = Input.GetKey(KeyCode.W) ? 1 : 0,
            down = Input.GetKey(KeyCode.S) ? -1 : 0,
            left = Input.GetKey(KeyCode.A) ? -1 : 0,
            right = Input.GetKey(KeyCode.D) ? 1 : 0;
        return new Vector2(left + right, up + down);
    }
}
