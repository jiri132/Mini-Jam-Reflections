using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [Header("PLAYING FIELD")]
    public Vector2 PlayingField;
    public Mirror SpawningMirror;
    public Vector2 LocalPosition() {
        //getting the offset between walls
        Vector2 offset = new Vector2(
            Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, collisionMask).distance,
            Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, collisionMask).distance
            );

        //calculating the position inside of the playing field
        Vector2 localPos = new Vector2(
            PlayingField.x - offset.x,
            PlayingField.y - offset.y
            );

        //returning the calculated localspace
        return localPos;
    }

    [Header("COLISSION MASK")]
    public LayerMask collisionMask;

    public Vector2 GetPlayingField(Transform player)
    {
        float x = Physics2D.Raycast(player.position, Vector2.left, Mathf.Infinity, collisionMask).distance + Physics2D.Raycast(player.position, Vector2.right, Mathf.Infinity, collisionMask).distance;
        float y = Physics2D.Raycast(player.position, Vector2.up, Mathf.Infinity, collisionMask).distance + Physics2D.Raycast(player.position, Vector2.down, Mathf.Infinity, collisionMask).distance;

        return new Vector2(x, y);
    }



    #region Spawning
    private Vector2[] Directions = new Vector2[4] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };

    public void GetReflectionMirrors(Player player)
    {
        foreach (Vector2 dir in Directions)
        {
            Debug.Log(dir);
            Debug.DrawRay(this.transform.position, dir * 100, Color.cyan, 3);
            Mirror mirror = Physics2D.Raycast(this.transform.position, dir, Mathf.Infinity, collisionMask).transform.GetComponent<Mirror>();
            if (mirror == null || player.SpawningMirror == mirror) { continue; }
            
            //GameManager.Instance._castedMirrors.Add(mirror);
            Vector2 mirrorPos = mirror.transform.position + (Vector3)dir;

            float x = 0, y = 0;

            if (dir == Vector2.left || dir == Vector2.right)
            {
                x = Physics2D.Raycast(mirrorPos, dir, Mathf.Infinity, collisionMask).distance;
                y = Physics2D.Raycast(mirrorPos, Vector2.up, Mathf.Infinity, collisionMask).distance + Physics2D.Raycast(mirrorPos, Vector2.down, Mathf.Infinity, collisionMask).distance;
            }
            else if (dir == Vector2.up || dir == Vector2.down)
            {
                x = Physics2D.Raycast(mirrorPos, Vector2.left, Mathf.Infinity, collisionMask).distance + Physics2D.Raycast(mirrorPos, Vector2.right, Mathf.Infinity, collisionMask).distance;
                y = Physics2D.Raycast(mirrorPos, dir, Mathf.Infinity, collisionMask).distance;
            }

            Vector2 playingField = new Vector2(x, y);
            GameManager.Instance.Fields.Add(playingField);

            //setting all the reflections values
            ReflectionController rc = Instantiate(GameManager.Instance._reflectionPrefab,Vector2.zero,Quaternion.identity).GetComponent<ReflectionController>();
            rc.PlayingField = playingField;
            rc.SpawningMirror = mirror;
            rc._player = player;
            if (mirror._mirrorDirection == Mirror.mirrorDirection.Horizontal) { rc.reflectionType = ReflectionController.ReflectionType.Vertical; }
            else { rc.reflectionType = ReflectionController.ReflectionType.Horizontal; }

            SpriteRenderer sr = rc.GetComponent<SpriteRenderer>();
            sr.color = player.GetComponent<SpriteRenderer>().color;
            sr.color = new Color(sr.color.r * 0.5f, sr.color.g * 0.5f, sr.color.b * 0.5f);

            GameManager.Instance._reflections.Add(rc);
        }
    }

    

    #endregion

}
