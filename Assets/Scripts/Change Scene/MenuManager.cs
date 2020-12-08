using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of creating undestrutible GameObjects, set the background music and its volume and save in PlayerPrefs the volume and the current scene index
/// </summary>

public class MenuManager : MonoBehaviour
{
    private GameObject soundManager;

    /// <summary>
    /// Function that is called right after the scene is loaded, make some indestructible GameObjects which will be used in every scene, set a fixed resolution
    /// and start the background music
    /// </summary>
    private void Awake()
    {
        Screen.SetResolution(768, 800, false);
        soundManager = GameObject.Find("SoundManager");
        DontDestroyOnLoad(soundManager);
        DontDestroyOnLoad(GameObject.Find("ChargeCanvas"));
        DontDestroyOnLoad(GameObject.Find("SceneLoader"));
        DontDestroyOnLoad(GameObject.Find("InventoryManager"));
        DontDestroyOnLoad(GameObject.Find("PauseCanvas"));

        if (!MultipleResources.musicStarted)
        {
            startMusic();
        }
    }

    /// <summary>
    /// Method that is called before the first frame update and save in PlayerPrefs the current scene index as an int
    /// </summary>
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int sceneIndex = currentScene.buildIndex;
        PlayerPrefs.SetInt("CurrentScene", sceneIndex);
    }

    /// <summary>
    /// Method that is responsible for starting the background music and call another method to set the volume
    /// </summary>
    private void startMusic()
    { 
        setMusic();
        soundManager.GetComponent<SoundManager>().PlaySoundClip("Music", Resources.Load<AudioClip>("Sounds/Background Music/Broken"), Camera.main.transform.position, true, PlayerPrefs.GetFloat("NormalBackgroundVolume"));
        DontDestroyOnLoad(GameObject.Find("Music"));

        MultipleResources.musicStarted = true;
    }


    /// <summary>
    /// Method that establish the background music volume and save those values in PlayerPrefs
    /// </summary>
    private void setMusic()
    {
        if (PlayerPrefs.GetFloat("MusicVolume") == 0)
        {
            PlayerPrefs.SetFloat("MasterVolume", 100);
            PlayerPrefs.SetFloat("MusicVolume", 100);
            PlayerPrefs.SetFloat("SoundsVolume", 100);
        }

        float normalVolume = (PlayerPrefs.GetFloat("MusicVolume") / 1250);
        float reducedVolume = normalVolume / 2;

        PlayerPrefs.SetFloat("NormalBackgroundVolume", normalVolume);
        PlayerPrefs.SetFloat("ReducedBackgroundVolume", reducedVolume);
    }
}