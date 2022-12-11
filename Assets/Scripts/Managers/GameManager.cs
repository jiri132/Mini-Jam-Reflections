using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Librarys.Singleton;


public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject _reflectionPrefab;
    [SerializeField] private Mirror[] _mirrors;



    private void OnLevelWasLoaded(int level)
    {
        if (level == 0) { return; }

        _mirrors = FindObjectsOfType<Mirror>();

        foreach (Mirror mirror in _mirrors)
        {
            if (mirror._mirrorDirection == Mirror.mirrorDirection.Horizontal)
            {
                 Instantiate(_reflectionPrefab).GetComponent<ReflectionController>().reflectionType = ReflectionController.ReflectionType.Vertical; 
            }else
            {
                Instantiate(_reflectionPrefab).GetComponent<ReflectionController>().reflectionType = ReflectionController.ReflectionType.Horizontal;
            }
        }
    }

        
}
