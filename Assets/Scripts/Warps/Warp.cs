using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage the warp beetwen places in the same scene
/// </summary>
public class Warp : MonoBehaviour
{
    public GameObject warpTarget, targetMap, soundManager;
    public string musicName;

    public float newHeight;
    public float cameraSize;
    public float newVelocity;

    private bool turnToBlack, endFadingColor;
    private GameObject chargeCanvas, chargePanel;

    /// <summary>
    /// Function that is called right after the scene is loaded. Get the SoundManager, disable the warps area image and get the ChargeCanvas 
    /// </summary>
    private void Awake()
    {
        soundManager = GameObject.Find("SoundManager");
        
        //Disable the warp area image
        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        chargeCanvas = GameObject.Find("ChargeCanvas");
        chargePanel = chargeCanvas.gameObject.transform.GetChild(0).gameObject;
    }

    /// <summary>
    /// Change the music clip and volume if needed, change the player height, velocity and camera size if needed and start charge animation before the warp
    /// </summary>
    /// <param name="collision">Gameobject that enter the warp area</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Start charge animation before the warp
        if (collision.tag == "Player")
        {
            endFadingColor = false;
            turnToBlack = true;

            StartCoroutine(FadeCanvas(collision));
        }
    }

    private void StartChanges(Collider2D collision)
    {
        // Change the music clip or volume
        if (musicName != null && !musicName.Equals(""))
        {
            if (musicName.Equals("Shadows All Around"))
            {
                float normalVolume = (PlayerPrefs.GetFloat("MusicVolume") / 1250);
                float reducedVolume = normalVolume / 2;

                PlayerPrefs.SetFloat("NormalBackgroundVolume", normalVolume);
                PlayerPrefs.SetFloat("ReducedBackgroundVolume", reducedVolume);
            }
            else if (musicName.Equals("Loop_Market_Day"))
            {
                float normalVolume = (PlayerPrefs.GetFloat("MusicVolume") / 2500);
                float reducedVolume = normalVolume / 2;

                PlayerPrefs.SetFloat("NormalBackgroundVolume", normalVolume);
                PlayerPrefs.SetFloat("ReducedBackgroundVolume", reducedVolume);
            }

            soundManager.GetComponent<SoundManager>().manageBackgroundMusic("Music", Resources.Load<AudioClip>("Sounds/Background Music/" + musicName), PlayerPrefs.GetFloat("NormalBackgroundVolume"));
        }

        // Change the player height, velocity and camera Size
        if (newHeight != 0 && newVelocity != 0 && cameraSize != 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<player>().velocity = newVelocity;
            collision.transform.localScale = new Vector3(newHeight, newHeight, newHeight);
            Camera.main.orthographicSize = cameraSize;
        }

        // Make the warp tp
        collision.transform.position = warpTarget.transform.GetChild(0).transform.position;
        Camera.main.GetComponent<CameraMovement>().setCameraBounds(targetMap);
    }

    /// <summary>
    /// Method that makes the transition of the loading panel from transparent to opaque and vice versa when the warp is done
    /// </summary>
    /// <param name="collision">Gameobject that enter the warp area</param>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator FadeCanvas(Collider2D collision)
    {
        float fadeTime = 1.2f;
        Image canvasImage = chargePanel.GetComponent<Image>();
        chargePanel.SetActive(true);

        try
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<player>().isTalking_or_isReading = true;
        }
        catch (Exception)
        { }

        // fade from transparent to opaque
        if (turnToBlack)
        {
            // loop over 1.2 second
            for (float currentFade = 0; currentFade <= fadeTime; currentFade += Time.deltaTime)
            {
                // set color with currentFade as alpha
                canvasImage.color = new Color(0, 0, 0, currentFade);
                yield return null;
            }

            endFadingColor = true;

            StartChanges(collision);

            // Makes the transition of the loading panel from opaque to transparent
            endFadingColor = false;
            turnToBlack = false;
            StartCoroutine(FadeCanvas(collision));
        }
        else
        {
            // loop over 1.2 second backwards
            for (float currentFade = fadeTime; currentFade >= 0; currentFade -= Time.deltaTime)
            {
                // set color with i as alpha
                canvasImage.color = new Color(0, 0, 0, currentFade);
                yield return null;
            }

            chargePanel.SetActive(false);
            endFadingColor = true;

            try
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<player>().isTalking_or_isReading = false;
            }
            catch (Exception)
            { }
        }
    }
}
