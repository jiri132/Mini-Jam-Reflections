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
}
