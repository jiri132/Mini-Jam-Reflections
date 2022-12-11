using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Librarys.Singleton;


public class GameManager : Singleton<GameManager>
{
    public GameObject _reflectionPrefab;
    public List<Mirror> _castedMirrors;
    public List<Vector2> Fields = new List<Vector2>();
    public List<ReflectionController> _reflections = new List<ReflectionController>();

}
