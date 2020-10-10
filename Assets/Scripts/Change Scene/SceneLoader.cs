using System;
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
/// This class is in charge of load a new scene, show the charge panel when it's charging and fade away the charge panel when the loading is done
/// </summary>
public class SceneLoader : MonoBehaviour
{
    private bool turnToBlack;
    private bool endFadingColor;
    private bool chargingScene;
    private GameObject chargeCanvas;
    private GameObject chargePanel;
    private GameObject chargeSlider;
    private GameObject chargeText;

    public bool ChargingScene { get => chargingScene; set => chargingScene = value; }

    /// <summary>
    /// Method that is called before the first frame update the scene is loaded and get the charge panel, slider and text GameObjects from the scene
    /// </summary>
    private void Start()
    {
        ChargingScene = false;
        chargeCanvas = GameObject.Find("ChargeCanvas");
        chargePanel = chargeCanvas.gameObject.transform.GetChild(0).gameObject;
        chargeSlider = chargeCanvas.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        chargeText = chargeCanvas.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
    }

    /// <summary>
    /// Start the load of a new scene, save the last scene index as an int in PlayerPrefs and unable the player movement
    /// </summary>
    /// <param name="sceneNumber">int that represent the scene that will load</param>
    public void LoadNewScene(int sceneNumber)
    {
        ChargingScene = true;
        Scene currentScene = SceneManager.GetActiveScene();
        int sceneIndex = currentScene.buildIndex;
        PlayerPrefs.SetInt("LastScene", sceneIndex);

        try
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<player>().isTalking_or_isReading = true;
        }
        catch (Exception)
        { }

        chargePanel.SetActive(true);
        endFadingColor = false;
        turnToBlack = true;

        StartCoroutine(FadeCanvas());
        StartCoroutine(LoadScene(sceneNumber));
    }

    /// <summary>
    /// Method that makes the transition of the loading panel from transparent to opaque and vice versa when the scene finish loading
    /// </summary>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator FadeCanvas()
    {
        float fadeTime = 1.2f;
        Image canvasImage = chargePanel.GetComponent<Image>();
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

            chargeSlider.SetActive(true);
            chargeText.SetActive(true);
            endFadingColor = true;
        }
        // fade from opaque to transparent
        else
        {
            chargeText.SetActive(false);
            chargeSlider.SetActive(false);

            // loop over 1.2 second backwards
            for (float currentFade = fadeTime; currentFade >= 0; currentFade -= Time.deltaTime)
            {
                // set color with currentFade as alpha
                canvasImage.color = new Color(0, 0, 0, currentFade);
                yield return null;
            }

            chargePanel.SetActive(false);
            endFadingColor = true;
        }
    }

    /// <summary>
    /// Load the new scene asynchronous, unity will uncharge the current scene and start loading a new scene, when the scene is loaded the charge panel will fade away
    /// </summary>
    /// <param name="sceneNumber">int that represent the scene that will load</param>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator LoadScene(int sceneNumber)
    {
        chargeText.GetComponent<Text>().text = "0 %";
        chargeSlider.GetComponent<Slider>().value = 0;

        do
        {
            yield return null;
        } while (!endFadingColor);

        yield return null;

        // Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneNumber);

        // Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;

        // When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            float progress = asyncOperation.progress;
            float percentageProgress = asyncOperation.progress * 100;
            chargeText.GetComponent<Text>().text = percentageProgress + " %";
            chargeSlider.GetComponent<Slider>().value = progress;

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                // Finish the load animation
                chargeSlider.GetComponent<Slider>().value = 0.9f;
                progress = 0.9f;
                asyncOperation.allowSceneActivation = true;

                do
                {
                    percentageProgress++;
                    progress += 0.01f;
                    chargeText.GetComponent<Text>().text = percentageProgress + " %";
                    chargeSlider.GetComponent<Slider>().value = progress;
                    yield return new WaitForSeconds(0.05f);
                } while (percentageProgress < 100);

                chargeText.GetComponent<Text>().text = "Carga Completa";
                yield return new WaitForSeconds(1.5f);

                endFadingColor = false;
                turnToBlack = false;
                StartCoroutine(FadeCanvas());

                do
                {
                    yield return null;
                } while (!endFadingColor);

                try
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<player>().isTalking_or_isReading = false;
                }
                catch (Exception)
                { }
                chargingScene = false;
            }

            yield return null;
        }
    }
}