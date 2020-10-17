using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// Class that read all the phrases from a json file, save them into multiple Dictionary and select the phrases from those Dictionary when needed
/// </summary>
public class Collectables_Objects_TextSelector : MonoBehaviour
{
    private string textLanguage = "";
    private bool isCollectable;
    private Dictionary<string, List<string>> interact_ObjectsDictionary;
    private Dictionary<string, string> collectable_ObjectsDictionary;

    public bool IsCollectable { get => isCollectable; set => isCollectable = value; }

    /// <summary>
    /// Seach in the dictionary the phrases of the object
    /// </summary>
    /// <param name="currentObject">String that contains the key of the object you are looking for in the dictionary</param>
    /// <returns>Return the value associated to the key, if they key is not found returns null</returns>
    public List<string> SelectObjectPhrases(string currentObject)
    {
        try
        {
            return interact_ObjectsDictionary[currentObject];
        }
        catch (System.Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Seach in the dictionary the phrases of the collectable
    /// </summary>
    /// <param name="collectable">String that contains the key of the collectacble you are looking for in the dictionary</param>
    /// <returns>Return the value associated to the key, if they key is not found returns null</returns>
    public string SelectCollectablePhrases(string collectable)
    {
        try
        {
            return collectable_ObjectsDictionary[collectable];
        }
        catch (System.Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Function that is called right after the scene is loaded. Open and read the Collectable_Objects phrases file according to the selected idiom. If no file is found
    /// then it will open the UI text file in spanish, then save the data of the file in multiple Dictionary
    /// </summary>
    private void Awake()
    {
        interact_ObjectsDictionary = new Dictionary<string, List<string>>();
        collectable_ObjectsDictionary = new Dictionary<string, string>();
        ReadCollectible_Interaction();
    }

    public void ReadCollectible_Interaction()
    {
        interact_ObjectsDictionary.Clear();
        collectable_ObjectsDictionary.Clear();

        if (PlayerPrefs.GetString("GameLanguage") != null && !PlayerPrefs.GetString("GameLanguage").Equals(""))
        {
            textLanguage = PlayerPrefs.GetString("GameLanguage");
        }
        else
        {
            PlayerPrefs.SetString("GameLanguage", "Spanish");
            textLanguage = PlayerPrefs.GetString("GameLanguage");
        }

        string currentSceneName = SceneManager.GetActiveScene().name;
        string filePath = Path.GetFullPath("./") + "Files\\Collectibles_Interactions" + textLanguage + ".json";
        //string filePath = Path.GetFullPath("./") + "Assets\\Files\\Collectibles_Interactions" + textIdiom + ".json";
        string jsonData;

        try
        {
            jsonData = File.ReadAllText(filePath);
        }
        catch (System.Exception)
        {
            filePath = Path.GetFullPath("./") + "Files\\Collectibles_InteractionsSpanish.json";
            //filePath = Path.GetFullPath("./") + "Assets\\Files\\Collectibles_InteractionsSpanish.json";
            jsonData = File.ReadAllText(filePath);
        }

        Collectables_Interactions collectables_Interactions = JsonUtility.FromJson<Collectables_Interactions>(jsonData);
        collectables_Interactions.selectClass(ref interact_ObjectsDictionary, ref collectable_ObjectsDictionary);
    }
}

/// <summary>
/// Class used to read data from the json file, holds the phases of the objects you can interact
/// </summary>
[System.Serializable]
public class Interact_Objects
{
    public string ObjectName;
    public List<string> ObjectPhrases;

    /// <summary>
    /// Add to the interact_ObjectsDictionary Dictionary all the phrases related to objects you can interact
    /// </summary>
    /// <param name="interact_ObjectsDictionary">Dictionary with all the objects phrases from the json file</param>
    public void IterateObjectSentences(ref Dictionary<string, List<string>> interact_ObjectsDictionary)
    {
        interact_ObjectsDictionary.Add(ObjectName, ObjectPhrases);
    }
}

/// <summary>
/// Class used to read data from the json file, holds the phases of the collectables you can interact
/// </summary>
[System.Serializable]
public class Collectable_Objects
{
    public string ObjectName;
    public string ObjectPhrase;

    /// <summary>
    /// Add to the collectable_ObjectsDictionary Dictionary all the phrases related to collectables you can interact
    /// </summary>
    /// <param name="collectable_ObjectsDictionary">Dictionary with all the collectables phrases from the json file</param>
    public void IterateCollectableSentences(ref Dictionary<string, string> collectable_ObjectsDictionary)
    {
        collectable_ObjectsDictionary.Add(ObjectName, ObjectPhrase);
    }
}

/// <summary>
/// Class used to read data from the json file, manage what class call
/// </summary>
[System.Serializable]
public class Collectables_Interactions
{
    public List<Interact_Objects> Interact_Objects;
    public List<Collectable_Objects> Collectable_Objects;

    public void selectClass(ref Dictionary<string, List<string>> interact_ObjectsDictionary, ref Dictionary<string, string> collectable_ObjectsDictionary)
    {
        // Call Collectable_Objects class
        foreach (Collectable_Objects collectable_Objects in Collectable_Objects)
        {
            collectable_Objects.IterateCollectableSentences(ref collectable_ObjectsDictionary);
        }
        // Call Interact_Objects class
        foreach (Interact_Objects interact_Objects in Interact_Objects)
        {
            interact_Objects.IterateObjectSentences(ref interact_ObjectsDictionary);
        }
    }
}