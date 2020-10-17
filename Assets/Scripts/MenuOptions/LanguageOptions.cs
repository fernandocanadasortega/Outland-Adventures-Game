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
public class LanguageOptions : MonoBehaviour
{
    private bool saveTextCoroutineON;
    private bool chargeLanguageMenu;
    private string selectedLanguage;

    private GameObject languagePanel;

    /// <summary>
    /// Function that is called right after the scene is loaded and load the language items text
    /// </summary>
    private void Awake()
    {
        loadLanguageText();

        languagePanel = this.gameObject;
        saveTextCoroutineON = false;
        chargeLanguageMenu = false;
    }

    /// <summary>
    /// Set the text language of Audio Options items
    /// </summary>
    private void loadLanguageText()
    {
        List<string> languageObjects = transform.parent.GetComponent<UI_LanguageSelector>().Select_UI_Objects("LanguagePanel");

        transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = languageObjects[0];
        transform.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>().text = languageObjects[5]; // save changes text
        transform.GetChild(6).GetComponent<TMPro.TextMeshProUGUI>().text = languageObjects[6];

        transform.GetChild(1).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = languageObjects[1];
        transform.GetChild(2).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = languageObjects[2];
        transform.GetChild(3).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = languageObjects[3]; // save changes button
        transform.GetChild(4).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = languageObjects[4];
    }

    /// <summary>
    /// Load PlayerPrefs values into the corresponding language items
    /// </summary>
    private void ReadPlayerPrefs()
    {
        if (PlayerPrefs.GetString("GameLanguage") != null && !PlayerPrefs.GetString("GameLanguage").Equals(""))
        {
            string language = PlayerPrefs.GetString("GameLanguage");

            switch (language)
            {
                case "Spanish":
                    selectedLanguage = "Spanish";
                    transform.GetChild(7).GetComponent<Toggle>().isOn = true;
                    break;

                case "English":
                    selectedLanguage = "English";
                    transform.GetChild(8).GetComponent<Toggle>().isOn = true;
                    break;
            }

            selectedLanguage = language;
        }
        else
        {
            PlayerPrefs.SetString("GameLanguage", "Spanish");
        }
    }

    /// <summary>
    /// Load audio options panel when open
    /// </summary>
    public void OpenLanguageMenu()
    {
        chargeLanguageMenu = true;
        ReadPlayerPrefs();
        chargeLanguageMenu = false;
    }

    /// <summary>
    /// Close language panel and show the previous panel (configuration panel)
    /// </summary>
    public void CloseLanguageMenu()
    {
        languagePanel.transform.GetChild(6).gameObject.SetActive(false);

        StopCoroutine(Show_RemoveSaveText());
        languagePanel.transform.GetChild(5).gameObject.SetActive(false);

        transform.GetChild(7).GetComponent<Toggle>().isOn = false;
        transform.GetChild(8).GetComponent<Toggle>().isOn = false;

        languagePanel.SetActive(false);
    }

    /// <summary>
    /// Change the selected language to Spanish
    /// </summary>
    public void ChangeToSpanish()
    {
        if (!chargeLanguageMenu)
        {
            selectedLanguage = "Spanish";
            transform.GetChild(7).GetComponent<Toggle>().isOn = true;
            transform.GetChild(8).GetComponent<Toggle>().isOn = false;

            CheckChanges();
        }
    }

    /// <summary>
    /// Change the selected language to English
    /// </summary>
    public void ChangeToEnglish()
    {
        if (!chargeLanguageMenu)
        {
            selectedLanguage = "English";
            transform.GetChild(7).GetComponent<Toggle>().isOn = false;
            transform.GetChild(8).GetComponent<Toggle>().isOn = true;

            CheckChanges();
        }
    }

    /// <summary>
    /// Check if the player change any value, if there are unsaved changes then show a message, if there is no unsaved changes then hide the message
    /// </summary>
    /// <returns>True if there is unsaved changes, false is there is any unsaved changes</returns>
    private bool CheckChanges()
    {
        GameObject unsavedText = languagePanel.transform.GetChild(6).gameObject;
        if (PlayerPrefs.GetString("GameLanguage").Equals(selectedLanguage))
        {
            unsavedText.SetActive(false);
            return false;
        }
        else
        {
            unsavedText.SetActive(true);
            return true;
        }
    }

    /// <summary>
    /// Check if there is unsaved changes and if there is then save the values in PlayerPrefs and show a message indicating
    /// the changes saved sucesfully
    /// </summary>
    public void SaveChanges()
    {
        bool unsavedChanges = CheckChanges();

        if (unsavedChanges)
        {
            PlayerPrefs.SetString("GameLanguage", selectedLanguage);


            if (saveTextCoroutineON)
            {
                StopCoroutine(Show_RemoveSaveText());
            }

            StartCoroutine(Show_RemoveSaveText());
            // GameObject.Find("PauseCanvas").GetComponent<UI_LanguageSelector>().textLanguage = PlayerPrefs.GetString("GameLanguage");
            if (GameObject.Find("PauseCanvas") != null) GameObject.Find("PauseCanvas").GetComponent<UI_LanguageSelector>().ReadGameGeneralData();
            if (GameObject.Find("Npc_Sentences") != null) GameObject.Find("Npc_Sentences").GetComponent<NPC_Sentences_Recoverer>().ReadNpcData();
            if (GameObject.Find("Collectable_Interaction_Sentences") != null) GameObject.Find("Collectable_Interaction_Sentences").GetComponent<Collectables_Objects_TextSelector>().ReadCollectible_Interaction();
            loadLanguageText();
        }
    }

    /// <summary>
    /// Manage the messages animations
    /// </summary>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator Show_RemoveSaveText()
    {
        saveTextCoroutineON = true;
        GameObject saveText = languagePanel.transform.GetChild(5).gameObject;
        languagePanel.transform.GetChild(6).gameObject.SetActive(false);
        saveText.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        saveText.SetActive(false);
        saveTextCoroutineON = false;
    }
}
