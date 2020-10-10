using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage the pause menu in game or in main menu, show the pause panel and stop or restart the game
/// </summary>
public class Pause_UI : MonoBehaviour
{
    private bool menuOpening_Closing;
    private bool configurationOpen;
    public bool gamePaused;
    public bool interactionInProcess;

    private bool checkEsc;
    private GameObject pausePanel;
    private SceneLoader sceneLoader;

    /// <summary>
    /// Function that is called right after the scene is loaded, get multiple GameObjects and initialize multiple variables
    /// </summary>
    private void Awake()
    {
        pausePanel = GameObject.Find("PauseCanvas").transform.GetChild(0).gameObject;
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();

        gamePaused = false;
        interactionInProcess = false;
        menuOpening_Closing = false;
        configurationOpen = false;
        checkEsc = true;
    }

    /// <summary>
    /// Call the function that manage the game pause in game
    /// </summary>
    public void StartPauseInteraction()
    {
        checkEsc = true;
        StartCoroutine(PauseInteraction());
    }

    /// <summary>
    /// Call the function that manage the game pause in main menu
    /// </summary>
    public void StartMenuPauseInteraction()
    {
        checkEsc = true;
        PauseInteractionActions();
        StartCoroutine(PauseInteraction());
    }

    /// <summary>
    /// Call the function that stop the game pause in game and in menu
    /// </summary>
    public void StopPauseInteraction()
    {
        // Stop game pause in main menu
        if (PlayerPrefs.GetInt("CurrentScene") == 0)
        {
            checkEsc = false;
        }
        // Stop game pause in game
        else
        {
            PauseInteractionActions();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator PauseInteraction()
    {
        while (true)
        {
            // Open pause menu in game
            try
            {
                if (Input.GetButton("Cancel") && !menuOpening_Closing && !configurationOpen && !sceneLoader.ChargingScene && !MultipleResources.PlayerIsTalking_or_isReading()
                    && !interactionInProcess)
                {
                    PauseInteractionActions();
                }
            }
            catch (System.Exception)
            {
                if (Input.GetButton("Cancel") && !menuOpening_Closing && !configurationOpen)
                {
                    PauseInteractionActions();
                    yield break;
                }
            }

            // Open pause menu in main menu
            if (!checkEsc)
            {
                PauseInteractionActions();
                yield break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// Unable or enable player movement, indicate if the game if pausing or exiting the pause and start charge fade animation
    /// </summary>
    public void PauseInteractionActions()
    {
        menuOpening_Closing = true;

        gamePaused = (gamePaused) ? gamePaused = false : gamePaused = true;

        try
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<player>().gamePaused = gamePaused;
        }
        catch (System.Exception)
        { }

        StartCoroutine(FadeCanvas());
    }

    /// <summary>
    /// Change background music volume and stop or restart the game
    /// </summary>
    private void PauseGame()
    {
        if (gamePaused)
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().manageBackgroundMusicVolume("Music", PlayerPrefs.GetFloat("ReducedBackgroundVolume"));
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            GameObject.Find("SoundManager").GetComponent<SoundManager>().manageBackgroundMusicVolume("Music", PlayerPrefs.GetFloat("NormalBackgroundVolume"));
        }
    }

    /// <summary>
    /// Fade charge canvas from transparent to opaque, show loading bar and text and vice versa
    /// </summary>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator FadeCanvas()
    {
        float fadeTime = 1f;
        float menuItemsFadeTime = fadeTime / 2;

        pausePanel.SetActive(true);
        Image canvasImage = pausePanel.GetComponent<Image>();

        // fade from transparent to opaque
        if (gamePaused)
        {
            // loop over 1 second
            for (float currentFade = 0; currentFade <= fadeTime; currentFade += Time.deltaTime)
            {
                // set color with currentFade as alpha
                canvasImage.color = new Color(0, 0, 0, currentFade);

                if (currentFade >= menuItemsFadeTime)
                {
                    ManageMainPauseMenu(true);
                    transform.GetChild(0).gameObject.GetComponent<PauseOptions>().enable_DiableReturnToMenu();
                }

                yield return null;
            }

            PauseGame();
        }
        else
        {
            PauseGame();
            ManageMainPauseMenu(false);

            for (float currentFade = fadeTime; currentFade >= 0; currentFade -= Time.deltaTime)
            {
                // set color with currentFade as alpha
                canvasImage.color = new Color(0, 0, 0, currentFade);

                if (currentFade <= menuItemsFadeTime)
                {
                    ManageMainPauseMenu(false);
                }

                yield return null;
            }

            pausePanel.SetActive(false);
        }

        menuOpening_Closing = false;
    }

    /// <summary>
    /// Show or hide the buttons of the pause menu
    /// </summary>
    /// <param name="state">Boolean - True to show, false to hide</param>
    private void ManageMainPauseMenu(bool state)
    {
        pausePanel.transform.GetChild(0).gameObject.SetActive(state);
        pausePanel.transform.GetChild(1).gameObject.SetActive(state);
        pausePanel.transform.GetChild(2).gameObject.SetActive(state);
        pausePanel.transform.GetChild(3).gameObject.SetActive(state);
    }

    /// <summary>
    /// Boolean that indicates if the configuration panel is open (true) or not (false)
    /// </summary>
    public void openConfiguration()
    {
        configurationOpen = (configurationOpen) ? false : true;
    }
}