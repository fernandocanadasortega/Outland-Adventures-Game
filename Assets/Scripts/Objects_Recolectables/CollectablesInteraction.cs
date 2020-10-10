using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage the collectible pick up
/// </summary>
public class CollectablesInteraction : MonoBehaviour
{
    private List<GameObject> interactedVillagers; // villagers of this scene who you can interact with a certain object
    private GameObject soundManager;
    private AudioClip collectibleSound;
    private float collectibleVolume;

    /// <summary>
    /// Method that is called before the first frame update. Check if the villagers or guards of the scene can give you something
    /// </summary>
    private void Start()
    {
        interactedVillagers = new List<GameObject>();
        soundManager = GameObject.Find("SoundManager");
        collectibleSound = Resources.Load<AudioClip>("Sounds/Objects Sounds/Collectible got sound");

        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        foreach (GameObject currentVillager in villagers)
        {
            try
            {
                if (currentVillager.GetComponent<VillagerInteraction>().giveObject)
                {
                    interactedVillagers.Add(currentVillager);
                }
            }
            catch (System.Exception)
            {
                if (currentVillager.GetComponent<GuardsInteraction>().giveObject)
                {
                    interactedVillagers.Add(currentVillager);
                }
            }
        }
    }

    /// <summary>
    /// Call StartPickUp method if the player interact with a Collectible
    /// </summary>
    /// <param name="collision">Gameobject that stay in the Collectible area</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetButton("Interaction") && !MultipleResources.PlayerIsTalking_or_isReading())
        {
            StartPickUp(this.tag);
        }
    }

    /// <summary>
    /// Make the player unable to move while you are picking up the Collectible, get the phrases of this Collectible, mark the Collectible as got
    /// and call GetCollectible method.
    /// </summary>
    /// <param name="currentCollectible">String, Tag of the Collectible you are interacting with</param>
    public void StartPickUp(string currentCollectible)
    {
        MultipleResources.PlayerIsTalking_or_isReading(true);
        GameObject sentences = GameObject.Find("Collectable_Interaction_Sentences");
        sentences.GetComponent<Collectables_Objects_TextSelector>().IsCollectable = true;
        string CollectablePhrase = sentences.GetComponent<Collectables_Objects_TextSelector>().SelectCollectablePhrases(currentCollectible);

        switch (currentCollectible)
        {
            case "Small_Key":
                Player_Objects.Small_Key = "got";
                break;

            case "Chest_Key":
                Player_Objects.Chest_Key = "got";
                foreach (GameObject currentVillager in interactedVillagers)
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
                break;

            case "Sword":
                Player_Objects.Sword = "got";
                break;
        }

        StartCoroutine(GetCollectable(CollectablePhrase));
    }

    /// <summary>
    /// Start the collectible pick up sound, show the phrase indicating which collectible you got and destroy that collectible so you can not pick it up again
    /// </summary>
    /// <param name="CollectiblePhrase">String, Phrase indicating which collectible you got</param>
    /// <returns></returns>
    private IEnumerator GetCollectable(string CollectiblePhrase)
    {
        // Start the sound
        SetCollectibleSoundVolume();
        soundManager.GetComponent<SoundManager>().manageBackgroundMusicVolume("Music", PlayerPrefs.GetFloat("ReducedBackgroundVolume"));
        soundManager.GetComponent<SoundManager>().PlaySoundClip("CollectibleGot", collectibleSound, MultipleResources.PlayerPosition(), false, collectibleVolume);

        yield return new WaitForSeconds(collectibleSound.length);

        // Show the text
        GameObject.Find("Collectable_Interaction_TextManager").GetComponent<Collectables_Objects_TextManager>().StartCollectableInteractionText(CollectiblePhrase);
        // Destroy collectible
        GameObject.Destroy(GameObject.FindGameObjectWithTag(gameObject.tag));
    }

    /// <summary>
    /// Set the collectible pick up sound
    /// </summary>
    public void SetCollectibleSoundVolume()
    {
        collectibleVolume = (PlayerPrefs.GetFloat("SoundsVolume") / 500); // 0,2 sound volume at max value
    }
}