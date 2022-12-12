using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Librarys.Singleton;


public class GameManager : Singleton<GameManager>
{
    public GameObject _reflectionPrefab;
    public List<ReflectionController> _reflections = new List<ReflectionController>();

    [SerializeField] private AudioClip MainMenuTheme;
    [SerializeField] private AudioClip PuzzleTheme;
    [SerializeField] private AudioClip SFX_Falling;

    public AudioSource[] audioSources;

    public void PlayDeathSound()
    {
        audioSources[1].clip = SFX_Falling;
        audioSources[1].Play();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            audioSources[0].clip = MainMenuTheme;
            audioSources[0].Play();
        }
        else
        {
            audioSources[0].clip = PuzzleTheme;
            audioSources[0].Play();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            audioSources[0].clip = MainMenuTheme;
            audioSources[0].Play();
        }
        else
        {
            audioSources[0].clip = PuzzleTheme;
            audioSources[0].Play();
        }
    }

}
