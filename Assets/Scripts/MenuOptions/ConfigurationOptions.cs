using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage the text idiom of Configuration Options items in the pause menu
/// </summary>
public class ConfigurationOptions : MonoBehaviour
{
    /// <summary>
    /// Function that is called right after the scene is loaded and manage the text language of the Configuration Options in the pause menu
    /// </summary>
    private void Awake()
    {
        SetLanguageText();
    }

    /// <summary>
    /// Set the text language of Configuration Options items
    /// </summary>
    private void SetLanguageText()
    {
        List<string> configurationObjects = transform.parent.GetComponent<UI_LanguageSelector>().Select_UI_Objects("ConfigurationPanel");
        transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = configurationObjects[0];

        for (int currentPauseObject = 1; currentPauseObject < configurationObjects.Count; currentPauseObject++)
        {
            transform.GetChild(currentPauseObject).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = configurationObjects[currentPauseObject];
        }
    }

    private void OnEnable()
    {
        SetLanguageText();
    }
}
