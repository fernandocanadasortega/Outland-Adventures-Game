using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class NPC_Sentences_Recoverer : MonoBehaviour
{
    public string npcLanguage = "";
    private Dictionary<string, List<string>> NpcPhrasesDictionary;

    private void Awake()
    {
        NpcPhrasesDictionary = new Dictionary<string, List<string>>();

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

    public List<string> recoverFunctionSentences(string NpcFuncion)
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