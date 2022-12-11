using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private LayerMask _LayerWalls;
    [SerializeField] private Vector2 _playerField;

    [SerializeField] private Vector2 _ownField;
    [SerializeField] private Vector2 _fieldRatio;

    private void Awake() => _player = FindObjectOfType<PlayerController>();

    private void Start()
    {
        //get the field sizes
        _playerField = GetPlayingField(_player.transform);
        _ownField = GetPlayingField(this.transform);

        //get  the ratio between movement
        _fieldRatio = _ownField / _playerField;
    }

    private Vector2 GetPlayingField(Transform player)
    {
        float x = Physics2D.Raycast(_player.transform.position, Vector2.up, Mathf.Infinity, _LayerWalls).distance + Physics2D.Raycast(player.position, Vector2.down, Mathf.Infinity, _LayerWalls).distance + player.localScale.x;
        float y = Physics2D.Raycast(player.position, Vector2.left, Mathf.Infinity, _LayerWalls).distance + Physics2D.Raycast(player.position, Vector2.right, Mathf.Infinity, _LayerWalls).distance + player.localScale.y;

        return new Vector2(x, y);
    }

}
