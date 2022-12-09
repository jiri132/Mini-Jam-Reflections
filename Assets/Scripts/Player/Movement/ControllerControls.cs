using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerControls : MovementManager
{
    public override Vector2 Direction()
    {
        float x = Input.GetAxis("Horizontal"),
              y = Input.GetAxis("Vertical");

        return new Vector2(x, y);
    }
}
