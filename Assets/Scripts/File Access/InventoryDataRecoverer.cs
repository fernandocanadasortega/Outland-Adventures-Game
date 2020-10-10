using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// Class that reads the player inventory from a file and charges into Player_Objects class and save the progress back into the file when the player save the game
/// </summary>
public class InventoryDataRecoverer : MonoBehaviour
{
    private PlayerInventory playerInventory;
    private string filePath;
    private string jsonData;

    /// <summary>
    /// Function that is called right after the scene is loaded and open the file of the player inventory, if the file exist then the file is readen, if the file
    /// does not exist then unable "Continue Game" button in main menu
    /// </summary>
    private void Awake()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        filePath = Path.GetFullPath("./") + "Files\\PlayerInventory.json";
        //filePath = Path.GetFullPath("./") + "Assets\\Files\\PlayerInventory.json";

        try
        {
            jsonData = File.ReadAllText(filePath);
            playerInventory = JsonUtility.FromJson<PlayerInventory>(jsonData);
            GameProgress();
        }
        catch (System.Exception)
        {
            checkSaveFile(false, 0f, 0.2f);
        }
    }

    /// <summary>
    /// Read the player inventory file and charge into Player_Objects during the game session
    /// </summary>
    private void GameProgress()
    {
        Player_Objects.Small_Key = playerInventory.Small_Key;
        Player_Objects.Chest_Key = playerInventory.Chest_Key;
        Player_Objects.Sword = playerInventory.Sword;
    }

    /// <summary>
    /// Unable "Continue Game" button in main menu and makes the button image and text semitransparent
    /// </summary>
    /// <param name="interactable">Make the button interactable or not</param>
    /// <param name="imageAlpha">Set the button image alpha</param>
    /// <param name="textAlpha">Set the button text alpha</param>
    private void checkSaveFile(bool interactable, float imageAlpha, float textAlpha)
    {
        // Unable "Continue Game" button
        GameObject.Find("MainMenu").transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).GetComponent<Button>().interactable = interactable;

        // Make button image semitransparent
        Color imageColor = GameObject.Find("MainMenu").transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).GetComponent<Image>().color;
        imageColor = new Color(imageColor.r, imageColor.g, imageColor.a, imageAlpha);
        GameObject.Find("MainMenu").transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).GetComponent<Image>().color = imageColor;

        // Make button text semitransparent
        Color textColor = GameObject.Find("MainMenu").transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color;
        textColor = new Color(textColor.r, textColor.g, textColor.b, textAlpha);
        GameObject.Find("MainMenu").transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = textColor;
    }

    /// <summary>
    /// Create a new player inventory file if there isn´t one
    /// </summary>
    public void CreateSaveFile()
    {
        playerInventory = new PlayerInventory();

        playerInventory.Small_Key = "none";
        playerInventory.Chest_Key = "none";
        playerInventory.Sword = "none";

        jsonData = JsonUtility.ToJson(playerInventory);
        File.WriteAllText(filePath, jsonData);

        jsonData = File.ReadAllText(filePath);
        playerInventory = JsonUtility.FromJson<PlayerInventory>(jsonData);
        GameProgress();
        checkSaveFile(true, 1f, 1f);
    }

    /// <summary>
    /// Save the changes in Player_Objects back into the player inventory file when the player save the game and restart this script
    /// </summary>
    public void SaveProgress()
    {
        playerInventory.Small_Key = Player_Objects.Small_Key;
        playerInventory.Chest_Key = Player_Objects.Chest_Key;
        playerInventory.Sword = Player_Objects.Sword;

        jsonData = JsonUtility.ToJson(playerInventory);
        File.WriteAllText(filePath, jsonData);

        GameObject.Destroy(this.gameObject);
    }
}

/// <summary>
/// Class used to read and write data into a json file, holds the player inventory
/// </summary>
[System.Serializable]
public class PlayerInventory
{
    public string Sword;
    public string Small_Key;
    public string Chest_Key;
}
