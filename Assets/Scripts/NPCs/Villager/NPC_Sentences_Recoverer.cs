using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that reads the npc sentences from a file and charges into NpcPhrasesDictionary dictionary
/// </summary>
public class NPC_Sentences_Recoverer : MonoBehaviour
{
    private string npcLanguage = "";
    private Dictionary<string, List<string>> NpcPhrasesDictionary;

    /// <summary>
    /// Function that is called right after the scene is loaded, initialize the variables and read all the npc sentences
    /// </summary>
    private void Awake()
    {
        NpcPhrasesDictionary = new Dictionary<string, List<string>>();
        ReadNpcData();
    }

    /// <summary>
    /// Read all the npc sentences from a json file and save it into a local dictionary according to the current scene
    /// </summary>
    public void ReadNpcData()
    {
        NpcPhrasesDictionary.Clear();

        if (PlayerPrefs.GetString("GameLanguage") != null && !PlayerPrefs.GetString("GameLanguage").Equals(""))
        {
            npcLanguage = PlayerPrefs.GetString("GameLanguage");
        }
        else
        {
            PlayerPrefs.SetString("GameLanguage", "Spanish");
            npcLanguage = PlayerPrefs.GetString("GameLanguage");
        }

        string currentSceneName = SceneManager.GetActiveScene().name;
        string filePath = Path.GetFullPath("./") + "Files\\NpcSenteces" + npcLanguage + ".json";
        //string filePath = Path.GetFullPath("./") + "Assets\\Files\\NpcSenteces" + npcLanguage + ".json";
        string jsonData;

        try
        {
            jsonData = File.ReadAllText(filePath);
        }
        catch (System.Exception)
        {
            filePath = Path.GetFullPath("./") + "Files\\NpcSentecesSpanish.json";
            //filePath = Path.GetFullPath("./") + "Assets\\Files\\NpcSentecesSpanish.json";
            jsonData = File.ReadAllText(filePath);
        }

        SceneList sceneList = JsonUtility.FromJson<SceneList>(jsonData);
        sceneList.ListGameScenes(currentSceneName, NpcPhrasesDictionary);
    }

    /// <summary>
    /// Recover all the npc sentences according to the ncp function
    /// </summary>
    /// <param name="NpcFuncion">String, npc function</param>
    /// <returns>Npc sentences according to the ncp function</returns>    
    public List<string> RecoverFunctionSentences(string NpcFuncion)
    {
        try
        {
            return NpcPhrasesDictionary[NpcFuncion];
        }
        catch (System.Exception)
        {
            return null;
        }
    }
}

/// <summary>
/// Class used to read and write data into a json file, holds the npc sentences according to the npc function
/// </summary>
[System.Serializable]
public class NpcSentences
{
    public string NpcFuncion;
    public List<string> NpcPhrases;

    public void IterateNpc(Dictionary<string, List<string>> NpcPhrasesDictionary)
    {
        NpcPhrasesDictionary.Add(NpcFuncion, NpcPhrases);
    }
}

/// <summary>
/// Class used to read and write data into a json file, holds all the npc sentences in a certain scene
/// </summary>
[System.Serializable]
public class NpcList
{
    public string Scene;
    public List<NpcSentences> Npcs;

    public void ListNpcs(string currentSceneName, Dictionary<string, List<string>> NpcPhrasesDictionary)
    {
        if (Scene.Equals(currentSceneName))
        {
            foreach (NpcSentences currentNpc in Npcs)
            {
                currentNpc.IterateNpc(NpcPhrasesDictionary);
            }
        }
    }
}

/// <summary>
/// Class used to read and write data into a json file, holds all the npc sentences in the game
/// </summary>
[System.Serializable]
public class SceneList
{
    public List<NpcList> GameScenes;

    public void ListGameScenes(string currentSceneName, Dictionary<string, List<string>> NpcPhrasesDictionary)
    {
        foreach (NpcList currentGameScene in GameScenes)
        {
            currentGameScene.ListNpcs(currentSceneName, NpcPhrasesDictionary);
        }
    }
}