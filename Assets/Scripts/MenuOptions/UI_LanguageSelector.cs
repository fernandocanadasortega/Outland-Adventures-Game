using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// cambiar
/// Class that reads the player inventory from a file and charges into Player_Objects class and save the progress back into the file when the player save the game
/// </summary>
public class UI_LanguageSelector : MonoBehaviour
{
    public string textIdiom = "";
    private Dictionary<string, List<string>> UI_Objects;

    /// <summary>
    /// Seach in the dictionary the value of the key
    /// </summary>
    /// <param name="currentObject">String that contains the key of the item you are looking for in the dictionary</param>
    /// <returns>Return the value associated to the key, if they key is not found returns null</returns>
    public List<string> Select_UI_Objects(string currentObject)
    {
        try
        {
            return UI_Objects[currentObject];
        }
        catch (System.Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Function that is called right after the scene is loaded, open and read the UI text file according to the selected idiom. If no file is found
    /// then it will open the UI text file in spanish, then save the data of the file in a Dictionary
    /// </summary>
    private void Awake()
    {
        UI_Objects = new Dictionary<string, List<string>>();

        string filePath = Path.GetFullPath("./") + "Files\\GameGeneralData" + textIdiom + ".json";
        //string filePath = Path.GetFullPath("./") + "Assets\\Files\\GameGeneralData" + textIdiom + ".json";
        string jsonData;

        try
        {
            jsonData = File.ReadAllText(filePath);
        }
        catch (System.Exception)
        {
            filePath = Path.GetFullPath("./") + "Files\\GameGeneralDataSpanish.json";
            //filePath = Path.GetFullPath("./") + "Assets\\Files\\GameGeneralDataSpanish.json";
            jsonData = File.ReadAllText(filePath);
        }

        PauseCanvas canvas_Objects = JsonUtility.FromJson<PauseCanvas>(jsonData);
        canvas_Objects.Add_UI_Objects(ref UI_Objects);
    }
}

/// <summary>
/// Class used to read data from the json file, holds the UI interface texts
/// </summary>
[System.Serializable]
public class PauseCanvas
{
    public List<string> PausePanel;
    public List<string> ConfigurationPanel;
    public List<string> SoundOptionsPanel;
    public List<string> ControlsPanel;
    public List<string> CameraOptionsPanel;

    /// <summary>
    /// Read the UI interface texts and save it into the dictionary
    /// </summary>
    /// <param name="UI_Objects">Dictionary where the file text is saved</param>
    public void Add_UI_Objects(ref Dictionary<string, List<string>> UI_Objects)
    {
        UI_Objects.Add("PausePanel", PausePanel);
        UI_Objects.Add("ConfigurationPanel", ConfigurationPanel);
        UI_Objects.Add("SoundOptionsPanel", SoundOptionsPanel);
        UI_Objects.Add("ControlsPanel", ControlsPanel);
        UI_Objects.Add("CameraOptionsPanel", CameraOptionsPanel);
    }
}