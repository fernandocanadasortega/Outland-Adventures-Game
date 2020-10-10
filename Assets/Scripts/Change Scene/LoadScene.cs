using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of load the specific objects of multiple scenes according to the player inventory
/// </summary>
/// 
public class LoadScene
{
    /// <summary>
    /// Method that call the method to change the cameraSize according to the current scene and set the camera movement bounds according to the destinationMap of the scene
    /// </summary>
    /// <param name="sceneBuildIndex">int that represent the scene that will load</param>
    /// <param name="destinationMap">GameObject that represent the map you will transport to</param>
    public LoadScene(int sceneBuildIndex, GameObject destinationMap)
    {
        switch (sceneBuildIndex)
        {
            case 1:
                adjustCameraSettings(6);
                Camera.main.GetComponent<CameraMovement>().setCameraBounds(destinationMap);
                break;

            case 2:
                adjustCameraSettings(6);
                Camera.main.GetComponent<CameraMovement>().setCameraBounds(destinationMap);
                break;

            case 3:
                adjustCameraSettings(9);
                Camera.main.GetComponent<CameraMovement>().setCameraBounds(destinationMap);
                charge_Market();
                break;

            case 4:
                adjustCameraSettings(9);
                Camera.main.GetComponent<CameraMovement>().setCameraBounds(destinationMap);
                charge_Dojo();
                break;

            case 5:
                adjustCameraSettings(9);
                Camera.main.GetComponent<CameraMovement>().setCameraBounds(destinationMap);
                charge_VillageWall();
                break;
        }
    }

    /// <summary>
    /// Method that change the cameraSize according to the current scene
    /// </summary>
    /// <param name="cameraSize">float that contains the new cameraSize value</param>
    private void adjustCameraSettings(float cameraSize)
    {
        Camera.main.orthographicSize = cameraSize;
    }

    /// <summary>
    /// Method that check the player inventory to know if he got the required item for this scene or not
    /// </summary>
    private void charge_Market()
    {
        if (Player_Objects.Small_Key.Equals("got") || Player_Objects.Small_Key.Equals("used"))
        {
            GameObject.Destroy(GameObject.Find("key"));

            if (Player_Objects.Small_Key.Equals("used"))
            {
                GameObject warps = GameObject.Find("Warps");
                warps.transform.GetChild(0).gameObject.SetActive(true);
                warps.transform.GetChild(1).gameObject.SetActive(true);
                GameObject.Find("BlackMarket_Door").gameObject.transform.GetChild(1).gameObject.SetActive(true);
                GameObject.Find("BlackMarket_Door").gameObject.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Method that check the player inventory to know if he got the items in this scene or not
    /// </summary>
    private void charge_Dojo()
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");

        if (Player_Objects.Chest_Key.Equals("got") || Player_Objects.Chest_Key.Equals("used"))
        {
            foreach (GameObject currentVillager in villagers)
            {
                currentVillager.GetComponent<NPC_DialogueSelector>().npcState = "GiftGiven";

                try
                {
                    currentVillager.GetComponent<VillagerInteraction>().giveObject = false;
                }
                catch (System.Exception)
                {
                    currentVillager.GetComponent<GuardsInteraction>().giveObject = false;
                }
            }

            if (Player_Objects.Chest_Key.Equals("used"))
            {
                Sprite[] gameObjects = Resources.LoadAll<Sprite>("Sprites/objects/objects");
                GameObject.Find("Chest").GetComponent<BoxCollider2D>().enabled = false;
                GameObject.Find("Chest").GetComponent<SpriteRenderer>().sprite = gameObjects[6];
            }
        }
    }

    /// <summary>
    /// Method that check the player inventory to know if he got the required item for this scene or not
    /// </summary>
    private void charge_VillageWall()
    {
        if (Player_Objects.Sword.Equals("used"))
        {
            GameObject spikes = GameObject.Find("Spikes").gameObject;
            spikes.transform.GetChild(0).gameObject.SetActive(false);
            spikes.transform.GetChild(1).gameObject.SetActive(true);

            GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
            foreach (GameObject currentVillager in villagers)
            {
                currentVillager.GetComponent<NPC_DialogueSelector>().npcState = "ObjectHandOver";

                try
                {
                    currentVillager.GetComponent<VillagerInteraction>().needObject = false;
                }
                catch (System.Exception)
                {
                    currentVillager.GetComponent<GuardsInteraction>().needObject = false;
                }
            }
        }
    }
}