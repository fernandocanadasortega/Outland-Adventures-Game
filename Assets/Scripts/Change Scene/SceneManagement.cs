using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of set the player position and orientation (face direction) in each scene and call another classes to set additional changes
/// </summary>
public class SceneManagement : MonoBehaviour
{
    private new Animator animation;
    private string faceDirection;
    private GameObject soundManager;
    private float normalVolume;
    private float reducedVolume;
    public GameObject destinationMap;

    /// <summary>
    /// Function that is called right after the scene is loaded, set the background music of the scene that will be charged, set the position and orientation (face direction)
    /// of the player according to the last scene the player had been in, save the scene index that will be charged as an int in PlayerPrefs and call LoadScene class to set
    /// additional changes
    /// </summary>
    void Awake()
    {
        soundManager = GameObject.Find("SoundManager");

        Scene currentScene = SceneManager.GetActiveScene();
        int sceneIndex = currentScene.buildIndex;
        int lastScene = PlayerPrefs.GetInt("LastScene");
        PlayerPrefs.SetInt("CurrentScene", sceneIndex);

        GameObject[] players = null;
        float xPosition = 0;
        float yPosition = 0;
        float zPosition = 0;

        players = GameObject.FindGameObjectsWithTag("Player");

        if (sceneIndex == 1)
        {
            normalVolume = (PlayerPrefs.GetFloat("MusicVolume") / 1250);
            reducedVolume = normalVolume / 2;

            PlayerPrefs.SetFloat("NormalBackgroundVolume", normalVolume);
            PlayerPrefs.SetFloat("ReducedBackgroundVolume", reducedVolume);

            soundManager.GetComponent<SoundManager>().manageBackgroundMusic("Music", Resources.Load<AudioClip>("Sounds/Background Music/Grassland Adventure"), normalVolume);
        }

        // warp from crossroads to spawn
        if (sceneIndex == 1 && lastScene == 2)
        {
            xPosition = 8.539654f;
            yPosition = 20.7529f;
            zPosition = -10f;

            faceDirection = "down";
        }

        // warp from spawn to crossroads
        else if (sceneIndex == 2 && lastScene == 1)
        {
            xPosition = 7.220622f;
            yPosition = 9.850777f;
            zPosition = -10f;

            faceDirection = "up";
        }

        // warp from crossroads to market
        else if (sceneIndex == 3 && lastScene == 2)
        {
            xPosition = 44.48f;
            yPosition = -10.24f;
            zPosition = -10f;

            faceDirection = "left";

            normalVolume = (PlayerPrefs.GetFloat("MusicVolume") / 2500);
            reducedVolume = normalVolume / 2;

            PlayerPrefs.SetFloat("NormalBackgroundVolume", normalVolume);
            PlayerPrefs.SetFloat("ReducedBackgroundVolume", reducedVolume);

            soundManager.GetComponent<SoundManager>().manageBackgroundMusic("Music", Resources.Load<AudioClip>("Sounds/Background Music/Loop_Market_Day"), normalVolume);
        }

        // warp from market to crossroads
        else if (sceneIndex == 2 && lastScene == 3)
        {
            xPosition = 2.45042f;
            yPosition = 14.81077f;
            zPosition = -10f;

            faceDirection = "right";

            normalVolume = (PlayerPrefs.GetFloat("MusicVolume") / 1250);
            reducedVolume = normalVolume / 2;

            PlayerPrefs.SetFloat("NormalBackgroundVolume", normalVolume);
            PlayerPrefs.SetFloat("ReducedBackgroundVolume", reducedVolume);

            soundManager.GetComponent<SoundManager>().manageBackgroundMusic("Music", Resources.Load<AudioClip>("Sounds/Background Music/Grassland Adventure"), normalVolume);
        }

        // warp from crossroads to dojo
        else if (sceneIndex == 4 && lastScene == 2)
        {
            xPosition = 11.08295f;
            yPosition = -27.06928f;
            zPosition = -10f;

            faceDirection = "right";

            normalVolume = (PlayerPrefs.GetFloat("MusicVolume") / 1250);
            reducedVolume = normalVolume / 2;

            PlayerPrefs.SetFloat("NormalBackgroundVolume", normalVolume);
            PlayerPrefs.SetFloat("ReducedBackgroundVolume", reducedVolume);

            soundManager.GetComponent<SoundManager>().manageBackgroundMusic("Music", Resources.Load<AudioClip>("Sounds/Background Music/Silver Sunrise"), normalVolume);
        }

        // warp from dojo to crossroads
        else if (sceneIndex == 2 && lastScene == 4)
        {
            xPosition = 12.17696f;
            yPosition = 14.51f;
            zPosition = -10f;

            faceDirection = "left";

            normalVolume = (PlayerPrefs.GetFloat("MusicVolume") / 1250);
            reducedVolume = normalVolume / 2;

            PlayerPrefs.SetFloat("NormalBackgroundVolume", normalVolume);
            PlayerPrefs.SetFloat("ReducedBackgroundVolume", reducedVolume);

            soundManager.GetComponent<SoundManager>().manageBackgroundMusic("Music", Resources.Load<AudioClip>("Sounds/Background Music/Grassland Adventure"), normalVolume);
        }

        // warp from crossroads to Village Wall
        else if (sceneIndex == 5 && lastScene == 2)
        {
            xPosition = 63.58f;
            yPosition = 4.790975f;
            zPosition = -10f;

            faceDirection = "up";
        }

        // warp from Village Wall to crossroads
        else if (sceneIndex == 2 && lastScene == 5)
        {
            xPosition = 7.216964f;
            yPosition = 19.29496f;
            zPosition = -10f;

            faceDirection = "down";
        }

        else
        {
            xPosition = 0f;
            yPosition = -3.98f;
            zPosition = -10f;
        }

        if (players != null)
        {
            foreach (GameObject currentPlayer in players)
            {
                animation = currentPlayer.GetComponent<Animator>();

                try
                {
                    if (faceDirection.Equals("up"))
                    {
                        animation.SetFloat("movementX", 0);
                        animation.SetFloat("movementY", 1);
                    }
                    else if (faceDirection.Equals("left"))
                    {
                        animation.SetFloat("movementX", -1);
                        animation.SetFloat("movementY", 0);
                    }
                    else if (faceDirection.Equals("right"))
                    {
                        animation.SetFloat("movementX", 1);
                        animation.SetFloat("movementY", 0);
                    }
                    else if (faceDirection.Equals("down"))
                    {
                        animation.SetFloat("movementX", 0);
                        animation.SetFloat("movementY", -1);
                    }
                }
                catch (System.Exception)
                {
                    animation.SetFloat("movementX", 0);
                    animation.SetFloat("movementY", 1);
                }


                currentPlayer.transform.position = new Vector3(xPosition, yPosition, zPosition);
            }
        }

        LoadScene loadScene = new LoadScene(sceneIndex, destinationMap);
    }
}