using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage main menu buttons (new game, load game, credits and exit game) operations
/// </summary>
public class GameManagerButtons : MonoBehaviour
{
    /// <summary>
    /// Create a new Player Progress file and call the function that start the game session
    /// </summary>
    public void CreateGame()
    {
        StartCoroutine(ChargeGameFile());
        GameObject.Find("InventoryManager").GetComponent<InventoryDataRecoverer>().CreateSaveFile();
    }

    /// <summary>
    /// Call the function that start the game session
    /// </summary>
    public void ChargeGame()
    {
        StartCoroutine(ChargeGameFile());
    }

    /// <summary>
    /// Disable main menu interface and call the function that start the credits
    /// </summary>
    public void ShowCredits()
    {
        transform.parent.transform.GetChild(0).gameObject.SetActive(false);
        transform.parent.transform.GetChild(1).gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        transform.parent.transform.GetChild(3).gameObject.GetComponent<CreditsManager>().StartCredits();
    }

    /// <summary>
    /// Exit the game
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Reproduce start game sound, allow the player to pause the game when the player press ESC key and start the game session
    /// </summary>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator ChargeGameFile()
    {
        yield return null;
        AudioClip startGameSound = Resources.Load<AudioClip>("Sounds/Other Sounds/enemy sound");
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySoundClip("GameStart", startGameSound, Camera.main.transform.position, false, 1);
        GameObject.Find("PauseCanvas").GetComponent<Pause_UI>().StartPauseInteraction();
        GameObject.Find("PauseCanvas").transform.GetChild(0).gameObject.GetComponent<PauseOptions>().enable_DiableReturnToMenu();
        GameObject.Find("SceneLoader").GetComponent<SceneLoader>().LoadNewScene(1);
    }
}