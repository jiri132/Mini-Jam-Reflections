using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionController : Player
{
    public enum ReflectionType { Horizontal , Vertical }


    [Header("MAIN OBJECT")]
    [SerializeField] public Player _player;

    [Header("COLLISION LAYER")]
    [SerializeField] private LayerMask _LayerWalls;

    [Header("REFLECTION SIDE")]
    [SerializeField] public ReflectionType reflectionType;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        this.transform.position = TranslatePlayerPosition(_player.transform);

        base.GetReflectionMirrors(this);
    }

    private void Update()
    {
        this.transform.position = TranslatePlayerPosition(_player.transform);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("VOID"))
        {
            _animator.Play(PlayerAnimator.Deathkey);
            GameManager.Instance.PlayDeathSound();
        }
    }

    public Vector2 TranslatePlayerPosition(Transform player)
    {
        Vector2 TranslatedPosition = new Vector2();
        Vector2 Ratio() => (_player.PlayingField - (Vector2)player.position) / _player.PlayingField ;

        //get the correct x and y base of reflection type
        if (reflectionType == ReflectionType.Horizontal)
        {
            TranslatedPosition.x = _player.PlayingField.x + Ratio().x * PlayingField.x;
            TranslatedPosition.y = player.position.y;
        }
        else if (reflectionType == ReflectionType.Vertical)
        {
            TranslatedPosition.x = player.position.x;
            TranslatedPosition.y = _player.PlayingField.y + Ratio().y * PlayingField.y;
        }

        return TranslatedPosition;
    }

    
}
