using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    private void Awake() => _player = FindObjectOfType<PlayerController>();
    


}
