using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionController : Player
{
    public enum ReflectionType { Horizontal , Vertical }


    [Header("MAIN OBJECT")]
    [SerializeField] private Player _player;

    [Header("COLLISION LAYER")]
    [SerializeField] private LayerMask _LayerWalls;

    

    public ReflectionType reflectionType;

    private void Awake() => _player = FindObjectOfType<PlayerController>();

    private void Start()
    {
        //get the field sizes
        _playerField = GetPlayingField(_player.transform);
        _ownField = GetPlayingField(this.transform);

        //get  the ratio between movement
        _fieldRatio = _ownField / _playerField;

        this.transform.position = TranslatePlayerPosition(_player.transform);
    }


    private void Update()
    {
        
    }
    
    public Vector2 TranslatePlayerPosition(Transform player)
    {
        //basic translation of player position
        Vector2 newPosition = _player.transform.position;

        //add the movement ratio to the position
        if (reflectionType == ReflectionType.Horizontal) { newPosition.x = this.transform.position.x * _fieldRatio.x * -1;  }
        else { newPosition.y = this.transform.position.y * _fieldRatio.y * -1; }

        return newPosition;
    }

    #region Fields
    [Header("FIELDS")]
    [SerializeField] private Vector2 _playerField;
    [SerializeField] private Vector2 _ownField;
    [SerializeField] private Vector2 _fieldRatio;

    private Vector2 GetPlayingField(Transform player)
    {
        float x = Physics2D.Raycast(player.position, Vector2.up, Mathf.Infinity, _LayerWalls).distance + Physics2D.Raycast(player.position, Vector2.down, Mathf.Infinity, _LayerWalls).distance + player.localScale.x;
        float y = Physics2D.Raycast(player.position, Vector2.left, Mathf.Infinity, _LayerWalls).distance + Physics2D.Raycast(player.position, Vector2.right, Mathf.Infinity, _LayerWalls).distance + player.localScale.y;

        return new Vector2(x, y);
    }
    #endregion
}
