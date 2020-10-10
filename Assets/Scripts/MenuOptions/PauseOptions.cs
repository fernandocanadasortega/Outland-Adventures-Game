using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// Class is in charge of manage the text idiom of Pause Options items in the pause menu, enable and disable 'return to menu' button and manage return to menu operation
/// </summary>
public class PauseOptions : MonoBehaviour
{
    /// <summary>
    /// Function that is called right after the scene is loaded and and manage the text idiom of the Pause Options in the pause menu
    /// </summary>
    private void Awake()
    {
        List<string> pauseObjects = transform.parent.GetComponent<UI_LanguageSelector>().Select_UI_Objects("PausePanel");
        transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = pauseObjects[0];

        for (int currentPauseObject = 1; currentPauseObject < pauseObjects.Count; currentPauseObject++)
        {
            transform.GetChild(currentPauseObject).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = pauseObjects[currentPauseObject];
        }
    }

    /// <summary>
    /// Enable or disable 'return to menu' button (and change alpha of the button image and the button text) in pause menu according to the current scene
    /// </summary>
    public void enable_DiableReturnToMenu()
    {
        // Disable 'return to menu' button and make semitransparent the button image and text (alpha: 0,2)
        if (PlayerPrefs.GetInt("CurrentScene") == 0)
        {
            Color imageColor = transform.GetChild(3).gameObject.GetComponent<Image>().color;
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.a, 0f);
            transform.GetChild(3).gameObject.GetComponent<Image>().color = imageColor;

            Color textColor = transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color;
            textColor = new Color(textColor.r, textColor.g, textColor.b, 0.2f);
            transform.GetChild(3).gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = textColor;

            transform.GetChild(3).gameObject.GetComponent<Button>().interactable = false;
        }
        // Enable 'return to menu' button and make opaque the button image and text (alpha: 1)
        else
        {
            Color imageColor = transform.GetChild(2).gameObject.GetComponent<Image>().color;
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.a, 1f);
            transform.GetChild(3).gameObject.GetComponent<Image>().color = imageColor;

            Color textColor = transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color;
            textColor = new Color(textColor.r, textColor.g, textColor.b, 1f);
            transform.GetChild(3).gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = textColor;

            transform.GetChild(3).gameObject.GetComponent<Button>().interactable = true;
        }
    }

    /// <summary>
    /// Play the background music of the main menu, unable the player to pause the game by pressing ESC key and load the main menu
    /// </summary>
    public void ReturnToMenu()
    {
        float normalVolume = (PlayerPrefs.GetFloat("MusicVolume") / 1250);
        float reducedVolume = normalVolume / 2;

        PlayerPrefs.SetFloat("NormalBackgroundVolume", normalVolume);
        PlayerPrefs.SetFloat("ReducedBackgroundVolume", reducedVolume);

        GameObject.Find("SoundManager").GetComponent<SoundManager>().manageBackgroundMusic("Music", Resources.Load<AudioClip>("Sounds/Background Music/Broken"), normalVolume);

        GameObject.Find("PauseCanvas").GetComponent<Pause_UI>().StopPauseInteraction();
        GameObject.Find("SceneLoader").GetComponent<SceneLoader>().LoadNewScene(0);
    }
}