using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class MovementManager
{
    /// <summary>
    /// Direction of the player
    /// </summary>
    /// <returns>Gives back the value of direction</returns>
    public abstract Vector2 Direction();

    /// <summary>
    /// Is the player on the ground?
    /// </summary>
    /// <returns>returns true if its on the ground </returns>
    public abstract bool OnGround();

    /// <summary>
    /// Speed of the player
    /// </summary>
    public float Speed = 10f;
}
