using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage the changeScene area and call the function to load a new scene
/// </summary>
public class ChangeScene : MonoBehaviour
{
    public int sceneNumber;

    /// <summary>
    /// Function that is called right after the scene is loaded and disable the image of the changeScene area
    /// </summary>
    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    /// <summary>
    /// Find SceneLoader Gameobject in order to load a new scene
    /// </summary>
    /// <param name="collision">Gameobject that enter the changeScene area</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("SceneLoader").GetComponent<SceneLoader>().LoadNewScene(sceneNumber);
    }
}
