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
/// This class is in charge of manage the text idiom of Camera Options items in the pause menu and manage every item functionality in Camera Options
/// </summary>
public class CameraOptions : MonoBehaviour
{
    private int cameraChase; // 0 - disabled     1 - enabled
    private float cameraChaseTime;

    private bool saveTextCoroutineON;
    private bool chargeCameraMenu;

    private GameObject cameraOptionsPanel;
    private GameObject[] cameraSliders_Input; // First object is cameraChase slider, second object is cameraChase time slider, third object is cameraChase time input

    /// <summary>
    /// Function that is called right after the scene is loaded and get from the Camera Options menu scene all the GameObjects, such as the buttons, sliders and inputs
    /// and save it into List and call loadCameraText function
    /// </summary>
    private void Awake()
    {
        loadCameraText();

        cameraOptionsPanel = this.gameObject;
        saveTextCoroutineON = false;
        chargeCameraMenu = false;
        cameraSliders_Input = new GameObject[3];

        cameraSliders_Input[0] = cameraOptionsPanel.transform.GetChild(2).gameObject;
        cameraSliders_Input[1] = cameraOptionsPanel.transform.GetChild(4).gameObject;
        cameraSliders_Input[2] = cameraOptionsPanel.transform.GetChild(5).gameObject;
    }

    /// <summary>
    /// Set the text idiom of Camera Options items
    /// </summary>
    private void loadCameraText()
    {
        List<string> cameraObjects = transform.parent.GetComponent<UI_LanguageSelector>().Select_UI_Objects("CameraOptionsPanel");

        for (int currentPauseObject = 0; currentPauseObject < 2; currentPauseObject++)
        {
            transform.GetChild(currentPauseObject).GetComponent<TMPro.TextMeshProUGUI>().text = cameraObjects[currentPauseObject];
        }

        transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = cameraObjects[2];
        transform.GetChild(8).GetComponent<TMPro.TextMeshProUGUI>().text = cameraObjects[5]; // save changes text
        transform.GetChild(9).GetComponent<TMPro.TextMeshProUGUI>().text = cameraObjects[6];


        transform.GetChild(6).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = cameraObjects[3]; // save changes button
        transform.GetChild(7).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = cameraObjects[4];
    }

    /// <summary>
    /// Load PlayerPrefs values into the corresponding camera options items
    /// </summary>
    private void ReadPlayerPrefs()
    {
        cameraChase = PlayerPrefs.GetInt("CameraChase");
        cameraChaseTime = PlayerPrefs.GetFloat("CameraChaseTime");

        if (cameraChaseTime == 0) cameraChaseTime = 0.3f;

        cameraSliders_Input[0].GetComponent<Slider>().value = cameraChase;
        cameraSliders_Input[1].GetComponent<Slider>().value = cameraChaseTime;
        cameraSliders_Input[2].GetComponent<InputField>().text = cameraChaseTime.ToString();

        // If you do not select camera chase option, disable camera chase settings
        if (PlayerPrefs.GetInt("CameraChase") == 0)
        {
            cameraSliders_Input[1].GetComponent<Slider>().interactable = false;
            cameraSliders_Input[2].GetComponent<InputField>().interactable = false;
        }
        // If you select camera chase option, enable camera chase settings
        else
        {
            cameraSliders_Input[1].GetComponent<Slider>().interactable = true;
            cameraSliders_Input[2].GetComponent<InputField>().interactable = true;
        }
    }

    /// <summary>
    /// Load camera options panel when open
    /// </summary>
    public void OpenCameraMenu()
    {
        chargeCameraMenu = true;
        ReadPlayerPrefs();
        chargeCameraMenu = false;
    }

    /// <summary>
    /// Close camera options panel and show the previous panel (configuration panel)
    /// </summary>
    public void CloseCameraMenu()
    {
        cameraOptionsPanel.transform.GetChild(9).gameObject.SetActive(false);

        StopCoroutine(Show_RemoveSaveText());
        cameraOptionsPanel.transform.GetChild(8).gameObject.SetActive(false);

        cameraOptionsPanel.SetActive(false);
    }

    /// <summary>
    /// Change the camera chase option slider, if this option is enabled then enable the camera chase settings,
    /// if this option is disabled then disable the camera chase settings
    /// </summary>
    public void ChangeCameraChaseSlider()
    {
        cameraChase = (int)cameraSliders_Input[0].GetComponent<Slider>().value;

        if (cameraChase == 0)
        {
            cameraSliders_Input[1].GetComponent<Slider>().interactable = false;
            cameraSliders_Input[2].GetComponent<InputField>().interactable = false;
        }
        else
        {
            cameraSliders_Input[1].GetComponent<Slider>().interactable = true;
            cameraSliders_Input[2].GetComponent<InputField>().interactable = true;
        }

        if (!chargeCameraMenu)
        {
            CheckChanges();
        }
    }

    /// <summary>
    /// Change camera chase setting input value according to the slider value
    /// </summary>
    public void ChangeCameraChaseTimeSlider()
    {
        cameraChaseTime = cameraSliders_Input[1].GetComponent<Slider>().value;
        cameraSliders_Input[2].GetComponent<InputField>().text = cameraChaseTime.ToString();

        if (!chargeCameraMenu)
        {
            CheckChanges();
        }
    }

    /// <summary>
    /// Change camera chase setting slider value according to the input value
    /// </summary>
    public void ChangeCameraChaseTimeInput()
    {
        // Set max and min values (min 0.3 and max 1)
        try
        {
            cameraChaseTime = float.Parse(cameraSliders_Input[2].GetComponent<InputField>().text);
            cameraChaseTime = Mathf.Clamp(cameraChaseTime, 0.3f, 1f);
        }
        catch (System.Exception)
        {
            cameraChaseTime = 0.1f;
        }

        // Change the values
        cameraSliders_Input[2].GetComponent<InputField>().text = cameraChaseTime.ToString();
        cameraSliders_Input[2].GetComponent<InputField>().caretPosition = cameraSliders_Input[2].GetComponent<InputField>().text.Length;

        cameraSliders_Input[1].GetComponent<Slider>().value = cameraChaseTime;
    }

    /// <summary>
    /// Check if the player change any value, if there are unsaved changes then show a message, if there is no unsaved changes then hide the message
    /// </summary>
    /// <returns>True if there is unsaved changes, false is there is any unsaved changes</returns>
    private bool CheckChanges()
    {
        GameObject unsavedText = cameraOptionsPanel.transform.GetChild(9).gameObject;
        if (PlayerPrefs.GetInt("CameraChase") != cameraChase || PlayerPrefs.GetFloat("CameraChaseTime") != cameraChaseTime)
        {
            unsavedText.SetActive(true);
            return true;
        }
        else
        {
            unsavedText.SetActive(false);
            return false;
        }
    }

    /// <summary>
    /// Check if there is unsaved changes and if there is then save the values in PlayerPrefs, update the camera values and show a message indicating
    /// the changes saved sucesfully
    /// </summary>
    public void SaveChanges()
    {
        bool unsavedChanges = CheckChanges();

        if (unsavedChanges)
        {
            PlayerPrefs.SetInt("CameraChase", cameraChase);
            PlayerPrefs.SetFloat("CameraChaseTime", cameraChaseTime);

            Camera.main.GetComponent<CameraMovement>().setCameraOptions();

            if (saveTextCoroutineON)
            {
                StopCoroutine(Show_RemoveSaveText());
            }

            StartCoroutine(Show_RemoveSaveText());
        }
    }

    /// <summary>
    /// Manage the messages animations
    /// </summary>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator Show_RemoveSaveText()
    {
        saveTextCoroutineON = true;
        GameObject saveText = cameraOptionsPanel.transform.GetChild(8).gameObject;
        cameraOptionsPanel.transform.GetChild(9).gameObject.SetActive(false);
        saveText.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        saveText.SetActive(false);
        saveTextCoroutineON = false;
    }
}