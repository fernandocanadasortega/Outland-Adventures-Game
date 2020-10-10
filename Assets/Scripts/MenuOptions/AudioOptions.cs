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
/// This class is in charge of manage the text idiom of Audio Options items in the pause menu and manage every item functionality in Audio Options
/// </summary>
public class AudioOptions : MonoBehaviour
{
    private float masterVolume;
    private float musicVolume;
    private float soundsVolume;

    private bool saveTextCoroutineON;
    private bool chargeAudioMenu;

    private GameObject soundOptionsPanel;
    private Dictionary<string, GameObject[]> soundSliders_Input;

    /// <summary>
    /// Function that is called right after the scene is loaded and get from the Audio Options menu scene all the GameObjects, such as the buttons, sliders and inputs
    /// and save it into List and call loadAudioText function
    /// </summary>
    private void Awake()
    {
        loadAudioText();

        soundOptionsPanel = this.gameObject;
        soundSliders_Input = new Dictionary<string, GameObject[]>();
        saveTextCoroutineON = false;
        chargeAudioMenu = false;

        int itemCounter = 0; // There is a text each 3 gameobjects and I only want sliders and input text so I will exclude those texts (First item is slider, second item is input and third item is text)
        GameObject soundSlider = null;
        GameObject soundInput = null;

        for (int currentGameObject = 2; currentGameObject < (soundOptionsPanel.transform.childCount - 3); currentGameObject++)
        {
            itemCounter++;
            if (itemCounter == 1)
            {
                soundSlider = soundOptionsPanel.transform.GetChild(currentGameObject).gameObject;
            }
            else if (itemCounter == 2)
            {
                soundInput = soundOptionsPanel.transform.GetChild(currentGameObject).gameObject;
            }
            else
            {
                itemCounter = 0;

                soundSliders_Input.Add(soundSlider.name.Remove(soundSlider.name.Length - 6), new GameObject[] { soundSlider, soundInput });
            }
        }
    }

    /// <summary>
    /// Set the text idiom of Audio Options items
    /// </summary>
    private void loadAudioText()
    {
        List<string> audioObjects = transform.parent.GetComponent<UI_LanguageSelector>().Select_UI_Objects("SoundOptionsPanel");

        transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = audioObjects[0];
        transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = audioObjects[1];
        transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = audioObjects[2];
        transform.GetChild(7).GetComponent<TMPro.TextMeshProUGUI>().text = audioObjects[3];
        transform.GetChild(12).GetComponent<TMPro.TextMeshProUGUI>().text = audioObjects[6]; // save changes text
        transform.GetChild(13).GetComponent<TMPro.TextMeshProUGUI>().text = audioObjects[7];

        transform.GetChild(10).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = audioObjects[4]; // save changes button
        transform.GetChild(11).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = audioObjects[5];
    }

    /// <summary>
    /// Load PlayerPrefs values into the corresponding audio options items
    /// </summary>
    private void ReadPlayerPrefs()
    {
        string[] playerPrefsNames = { "MasterVolume", "MusicVolume", "SoundsVolume" };
        int currentPlayerPrefsNames = 0;
        foreach (KeyValuePair<string, GameObject[]> currentEntry in soundSliders_Input)
        {
            if (PlayerPrefs.GetFloat(playerPrefsNames[currentPlayerPrefsNames]) == 0)
            {
                currentEntry.Value[0].GetComponent<Slider>().value = 100;
                currentEntry.Value[1].GetComponent<InputField>().text = "100";

                PlayerPrefs.SetFloat(playerPrefsNames[currentPlayerPrefsNames], 100);
            }
            else
            {
                currentEntry.Value[0].GetComponent<Slider>().value = PlayerPrefs.GetFloat(playerPrefsNames[currentPlayerPrefsNames]);
                currentEntry.Value[1].GetComponent<InputField>().text = PlayerPrefs.GetFloat(playerPrefsNames[currentPlayerPrefsNames]).ToString();
            }
            currentPlayerPrefsNames++;
        }
    }

    /// <summary>
    /// Load audio options panel when open
    /// </summary>
    public void OpenMusicMenu()
    {
        chargeAudioMenu = true;
        ReadPlayerPrefs();
        chargeAudioMenu = false;
    }

    /// <summary>
    /// Close audio options panel and show the previous panel (configuration panel)
    /// </summary>
    public void CloseMusicMenu()
    {
        soundOptionsPanel.transform.GetChild(13).gameObject.SetActive(false);

        StopCoroutine(Show_RemoveSaveText());
        soundOptionsPanel.transform.GetChild(12).gameObject.SetActive(false);

        soundOptionsPanel.SetActive(false);
    }

    /// <summary>
    /// Change master volume input value according to the master volume slider value
    /// </summary>
    public void ChangeMasterVolumeSlider()
    {
        GameObject[] masterItems;
        soundSliders_Input.TryGetValue("MasterVolume", out masterItems);

        masterVolume = (int) masterItems[0].GetComponent<Slider>().value;
        masterItems[1].GetComponent<InputField>().text = masterVolume.ToString();
        masterItems[1].GetComponent<InputField>().caretPosition = masterItems[1].GetComponent<InputField>().text.Length;

        setMaximumValues();

        if (!chargeAudioMenu)
        {
            CheckChanges();
        }
    }

    /// <summary>
    /// Change master volume slider value according to the master volume input value
    /// </summary>
    public void ChangeMasterVolumeInput()
    {
        GameObject[] masterItems;
        soundSliders_Input.TryGetValue("MasterVolume", out masterItems);

        // Set max and min values (min 1 and max 100)
        try
        {
            masterVolume = (int) Math.Truncate(double.Parse(masterItems[1].GetComponent<InputField>().text));
            masterVolume = Mathf.Clamp(masterVolume, 1, 100);
        }
        catch (System.Exception)
        {
            masterVolume = 1;
        }

        // Change the values
        masterItems[1].GetComponent<InputField>().text = masterVolume.ToString();
        masterItems[1].GetComponent<InputField>().caretPosition = masterItems[1].GetComponent<InputField>().text.Length;

        masterItems[0].GetComponent<Slider>().value = masterVolume;
    }

    /// <summary>
    /// Change music volume input value according to the music volume slider value
    /// </summary>
    public void ChangeMusicVolumeSlider()
    {
        GameObject[] musicItems;
        soundSliders_Input.TryGetValue("MusicVolume", out musicItems);

        musicVolume = (int)musicItems[0].GetComponent<Slider>().value;
        musicItems[1].GetComponent<InputField>().text = musicVolume.ToString();
        musicItems[1].GetComponent<InputField>().caretPosition = musicItems[1].GetComponent<InputField>().text.Length;

        setMaximumValues();

        if (!chargeAudioMenu)
        {
            CheckChanges();
        }
    }

    /// <summary>
    /// Change music volume slider value according to the music volume input value
    /// </summary>
    public void ChangeMusicVolumeInput()
    {
        GameObject[] musicItems;
        soundSliders_Input.TryGetValue("MusicVolume", out musicItems);

        // Set max and min values (min 1 and max 100)
        try
        {
            musicVolume = (int)Math.Truncate(double.Parse(musicItems[1].GetComponent<InputField>().text));
            musicVolume = Mathf.Clamp(musicVolume, 1, 100);
        }
        catch (System.Exception)
        {
            musicVolume = 1;
        }

        // Change the values
        musicItems[1].GetComponent<InputField>().text = musicVolume.ToString();
        musicItems[1].GetComponent<InputField>().caretPosition = musicItems[1].GetComponent<InputField>().text.Length;

        musicItems[0].GetComponent<Slider>().value = musicVolume;
    }

    /// <summary>
    /// Change sounds volume input value according to the sounds volume slider value
    /// </summary>
    public void ChangeSoundsVolumeSlider()
    {
        GameObject[] soundItems;
        soundSliders_Input.TryGetValue("SoundVolume", out soundItems);

        soundsVolume = (int)soundItems[0].GetComponent<Slider>().value;
        soundItems[1].GetComponent<InputField>().text = soundsVolume.ToString();
        soundItems[1].GetComponent<InputField>().caretPosition = soundItems[1].GetComponent<InputField>().text.Length;

        setMaximumValues();

        if (!chargeAudioMenu)
        {
            CheckChanges();
        }
    }

    /// <summary>
    /// Change sounds volume slider value according to the sounds volume input value
    /// </summary>
    public void ChangeSoundsVolumeInput()
    {
        GameObject[] soundItems;
        soundSliders_Input.TryGetValue("SoundVolume", out soundItems);

        // Set max and min values (min 1 and max 100)
        try
        {
            soundsVolume = (int)Math.Truncate(double.Parse(soundItems[1].GetComponent<InputField>().text));
            soundsVolume = Mathf.Clamp(soundsVolume, 1, 100);
        }
        catch (System.Exception)
        {
            soundsVolume = 1;
        }

        // Change the values
        soundItems[1].GetComponent<InputField>().text = soundsVolume.ToString();
        soundItems[1].GetComponent<InputField>().caretPosition = soundItems[1].GetComponent<InputField>().text.Length;

        soundItems[0].GetComponent<Slider>().value = soundsVolume;
    }

    /// <summary>
    /// Check if the music volume value or the sound volume value is bigger that the master volume value, if the number is bigger than master volume value
    /// then equals the two values
    /// </summary>
    private void setMaximumValues()
    {
        GameObject[] masterItems;
        GameObject[] musicItems;
        GameObject[] soundItems;

        soundSliders_Input.TryGetValue("MasterVolume", out masterItems);
        soundSliders_Input.TryGetValue("MusicVolume", out musicItems);
        soundSliders_Input.TryGetValue("SoundVolume", out soundItems);

        if (masterVolume < musicVolume)
        {
            musicItems[0].GetComponent<Slider>().value = masterVolume;
        }

        if (masterVolume < soundsVolume)
        {
            soundItems[0].GetComponent<Slider>().value = masterVolume;
        }
    }

    /// <summary>
    /// Check if the player change any value, if there are unsaved changes then show a message, if there is no unsaved changes then hide the message
    /// </summary>
    /// <returns>True if there is unsaved changes, false is there is any unsaved changes</returns>
    private bool CheckChanges()
    {
        GameObject unsavedText = soundOptionsPanel.transform.GetChild(13).gameObject;
        if (PlayerPrefs.GetFloat("MasterVolume") != masterVolume || PlayerPrefs.GetFloat("MusicVolume") != musicVolume || PlayerPrefs.GetFloat("SoundsVolume") != soundsVolume)
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
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("SoundsVolume", soundsVolume);


            if (GameObject.Find("PauseCanvas").GetComponent<Pause_UI>().gamePaused)
            {
                checkScene();
                GameObject.Find("SoundManager").GetComponent<SoundManager>().manageBackgroundMusicVolume("Music", PlayerPrefs.GetFloat("ReducedBackgroundVolume"));
            }
            else
            {
                checkScene();
                GameObject.Find("SoundManager").GetComponent<SoundManager>().manageBackgroundMusicVolume("Music", PlayerPrefs.GetFloat("NormalBackgroundVolume"));
            }

            if (saveTextCoroutineON)
            {
                StopCoroutine(Show_RemoveSaveText());
            }

            StartCoroutine(Show_RemoveSaveText());
        }
    }

    /// <summary>
    /// Check the scene you are when you save the volume changes, according to the scene the volume will be different
    /// </summary>
    private void checkScene()
    {
        float normalVolume;
        float reducedVolume;

        if (PlayerPrefs.GetInt("CurrentScene") == 3) // market
        {
            normalVolume = (PlayerPrefs.GetFloat("MusicVolume") / 2500);
            reducedVolume = normalVolume / 2;

            PlayerPrefs.SetFloat("NormalBackgroundVolume", normalVolume);
            PlayerPrefs.SetFloat("ReducedBackgroundVolume", reducedVolume);
        }
        else
        {
            normalVolume = (PlayerPrefs.GetFloat("MusicVolume") / 1250);
            reducedVolume = normalVolume / 2;

            PlayerPrefs.SetFloat("NormalBackgroundVolume", normalVolume);
            PlayerPrefs.SetFloat("ReducedBackgroundVolume", reducedVolume);
        }
    }

    /// <summary>
    /// Manage the messages animations
    /// </summary>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator Show_RemoveSaveText()
    {
        saveTextCoroutineON = true;
        GameObject saveText = soundOptionsPanel.transform.GetChild(12).gameObject;
        soundOptionsPanel.transform.GetChild(13).gameObject.SetActive(false);
        saveText.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        saveText.SetActive(false);
        saveTextCoroutineON = false;
    }
}