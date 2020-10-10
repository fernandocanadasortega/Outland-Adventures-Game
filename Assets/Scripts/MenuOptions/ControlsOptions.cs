using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage the text idiom of Control Options items in the pause menu
/// </summary>
public class ControlsOptions : MonoBehaviour
{
    /// <summary>
    /// Function that is called right after the scene is loaded and manage the text idiom of the Control Options in the pause menu
    /// </summary>
    private void Awake()
    {
        List<string> controlsObjects = transform.parent.GetComponent<UI_LanguageSelector>().Select_UI_Objects("ControlsPanel");

        for (int currentPauseObject = 0; currentPauseObject < 2; currentPauseObject++)
        {
            transform.GetChild(currentPauseObject).GetComponent<TMPro.TextMeshProUGUI>().text = controlsObjects[currentPauseObject];
        }

        transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = controlsObjects[2];
        transform.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>().text = controlsObjects[3];
        transform.GetChild(7).GetComponent<TMPro.TextMeshProUGUI>().text = controlsObjects[4];
        transform.GetChild(9).GetComponent<TMPro.TextMeshProUGUI>().text = controlsObjects[5];
        transform.GetChild(11).GetComponent<TMPro.TextMeshProUGUI>().text = controlsObjects[6];
        transform.GetChild(13).GetComponent<TMPro.TextMeshProUGUI>().text = controlsObjects[7];
        transform.GetChild(14).GetComponent<TMPro.TextMeshProUGUI>().text = controlsObjects[8];

        transform.GetChild(15).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = controlsObjects[9];
    }
}
