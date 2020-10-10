using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage the objects you can interact with, like doors or chests looked
/// </summary>
public class ObjectsInteraction : MonoBehaviour
{
    private string requiredObject;
    public BoxCollider2D objectCollider;
    private List<GameObject> interactedVillagers; // villagers of this scene who you can interact with a certain object
    private GameObject chargePanel;
    private GameObject soundManager;
    private AudioClip objectSound;
    private bool saveGameObject;
    private bool hasCharged;
    private bool turnToBlack;
    private bool endFadingColor;
    private float objectVolume;

    /// <summary>
    /// Method that is called before the first frame update. Check if the villagers or guards of the scene need something from you
    /// </summary>
    private void Start()
    {
        interactedVillagers = new List<GameObject>();
        chargePanel = GameObject.Find("ChargeCanvas").gameObject.transform.GetChild(0).gameObject;
        soundManager = GameObject.Find("SoundManager");
        endFadingColor = false;
        hasCharged = false;
        turnToBlack = false;

        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        foreach (GameObject currentVillager in villagers)
        {
            try
            {
                if (currentVillager.GetComponent<VillagerInteraction>().needObject)
                {
                    interactedVillagers.Add(currentVillager);
                }
            }
            catch (System.Exception)
            {
                if (currentVillager.GetComponent<GuardsInteraction>().needObject)
                {
                    interactedVillagers.Add(currentVillager);
                }
            }
        }
    }

    /// <summary>
    /// Check if the interactive object if a save progress object, if it is then start the open book animation
    /// </summary>
    /// <param name="collision">Gameobject that enter the interactive object area</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (this.tag == "SaveGame")
            {
                saveGameObject = true;
                Animator bookAnimator = GetComponent<Animator>();
                bookAnimator.SetBool("bookOpen", true);
            }
            else if (this.tag != "SaveGame")
            {
                saveGameObject = false;
            }
        }
    }

    /// <summary>
    /// Call GetObject method if the player interact with an interactive object
    /// </summary>
    /// <param name="collision">Gameobject that stay in the interactive object area</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!this.tag.Equals("Untagged") && collision.tag == "Player" && Input.GetButton("Interaction") && !MultipleResources.PlayerIsTalking_or_isReading())
        {
            GetObject(this.tag);
        }
    }

    /// <summary>
    /// Check if the interactive object if a save progress object, if it is then start the close book animation
    /// </summary>
    /// <param name="collision">Gameobject that exit the interactive object area</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && this.tag == "SaveGame")
        {
            Animator bookAnimator = GetComponent<Animator>();
            bookAnimator.SetBool("bookOpen", false);
        }
    }

    /// <summary>
    /// Make the player unable to move while you are interacting with the object, get the phrases of this interactive object, check the associated collectible status (got, used, etc),
    /// reduce the background music and call the TextManager to show the corresponding text
    /// </summary>
    /// <param name="currentInteractionObject">String, Tag of the object you are interacting with</param>
    public void GetObject(string currentInteractionObject)
    {
        hasCharged = false;

        MultipleResources.PlayerIsTalking_or_isReading(true);
        List<string> objectPhrases = RecoverPhrases(currentInteractionObject);

        // Check associated collectible status
        switch (currentInteractionObject)
        {
            case "BlackMarket_Door":
                requiredObject = Player_Objects.Small_Key;
                break;

            case "Sword_Chest":
                requiredObject = Player_Objects.Chest_Key;
                break;

            case "VillageWall_Knight":
                requiredObject = Player_Objects.Sword;
                break;

            default:
                requiredObject = "none";
                break;
        }

        soundManager.GetComponent<SoundManager>().manageBackgroundMusicVolume("Music", PlayerPrefs.GetFloat("ReducedBackgroundVolume"));

        // Show the text of generic interactive object
        if (!saveGameObject)
        {
            GameObject.Find("Collectable_Interaction_TextManager").GetComponent<Collectables_Objects_TextManager>().StartObjectInteractionText(objectPhrases, requiredObject, this);
        }
        // Show the text of save progress interactive object
        else
        {
            GameObject.Find("Collectable_Interaction_TextManager").GetComponent<Collectables_Objects_TextManager>().StartSaveText(objectPhrases, true, this);
        }
    }

    /// <summary>
    /// Recover the required phrases for this interactive object
    /// </summary>
    /// <param name="currentInteractionObject">String, Tag of the object you are interacting with</param>
    /// <returns>List<String>, Array with all the required phrases for this interactive object</returns>
    private List<string> RecoverPhrases(string currentInteractionObject)
    {
        GameObject sentences = GameObject.Find("Collectable_Interaction_Sentences");
        sentences.GetComponent<Collectables_Objects_TextSelector>().IsCollectable = false;
        return sentences.GetComponent<Collectables_Objects_TextSelector>().SelectObjectPhrases(currentInteractionObject);
    }

    /// <summary>
    /// Call SaveGame coroutine if the player interacted with a save progress object, if not then check if you are interacting with a NPC or with another object (like a door).
    /// If you are interacting with a NPC then go to the NPC data and recover the object the NPC need or give, if you are interating with another object then get the tag of the object
    /// you are interacting with. At the end the screen darkens
    /// </summary>
    public void StartChanges()
    {
        // Start Save Game process
        if (saveGameObject)
        {
            StartCoroutine(SaveGame());
        }
        // Start object interaction
        else
        {
            string currentInteractionObject;
            // If there are no villagers, you are interacting with another object, then get the object tag
            if (interactedVillagers.Count == 0)
            {
                currentInteractionObject = this.tag;
            }
            // If there are villagers the recover the collectible the villager require or give. Also the tag can be the proper tag of the NPC
            else
            {
                try
                {
                    currentInteractionObject = interactedVillagers[0].GetComponent<VillagerInteraction>().requiredObjectTag;
                }
                catch (System.Exception)
                {
                    currentInteractionObject = interactedVillagers[0].GetComponent<GuardsInteraction>().requiredObjectTag;
                }
            }

            StartCoroutine(BlackScreen(currentInteractionObject));
        }
    }

    /// <summary>
    /// Make the player unable to move or start the pause menu, start the FadeCanvas coroutine, when the coroutine is over (ChargePanel is opaque) then start the necessary changes
    /// and play the selected sound, when the sound ends then start the FadeCanvas coroutine again, when is over (ChangePanel is transparent) set the remaining changes and
    /// allow the player to move and start the pause menu
    /// </summary>
    /// <param name="currentInteractionObject">String, Tag of the object you are interacting with or tag of the collectible the NPC gives you</param>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator BlackScreen(string currentInteractionObject)
    {
        //Make the player unable to move or start the pause menu
        MultipleResources.PlayerIsTalking_or_isReading(true);
        GameObject.Find("PauseCanvas").GetComponent<Pause_UI>().interactionInProcess = true;

        // Start the FadeCanvas coroutine to turn the screen black
        chargePanel.SetActive(true);
        turnToBlack = true;
        StartCoroutine(FadeCanvas(chargePanel.GetComponent<Image>()));

        do
        {
            yield return new WaitForSeconds(0.2f);
            // Wait 0.2 seconds if FadeCanvas couroutine is still working
        } while (!endFadingColor);

        // FadeCanvas coroutine is over
        endFadingColor = false;

        SetChanges(currentInteractionObject);
        // Recover the corresponding sound of the interative object
        AdditionalChanges(currentInteractionObject, true);
        SetObjectsSoundVolume();
        // Play the sound
        soundManager.GetComponent<SoundManager>().PlaySoundClip("ObjectInteraction", objectSound, MultipleResources.PlayerPosition(), false, objectVolume);
        yield return new WaitForSeconds(objectSound.length);

        do
        {
            yield return new WaitForSeconds(0.5f);
            // Wait 0.5 seconds if the changes in SetChanges are not ready
        } while (!hasCharged);

        // Start the FadeCanvas coroutine to turn the screen back to normal
        turnToBlack = false;
        StartCoroutine(FadeCanvas(chargePanel.GetComponent<Image>()));

        do
        {
            yield return new WaitForSeconds(0.2f);
            // Wait 0.2 seconds if FadeCanvas couroutine is still working
        } while (!endFadingColor);

        // FadeCanvas coroutine is over
        chargePanel.SetActive(false);
        endFadingColor = false;

        // Set Additional changes
        AdditionalChanges(currentInteractionObject, false);
        GameObject.Find("PauseCanvas").GetComponent<Pause_UI>().interactionInProcess = false;
    }

    /// <summary>
    /// Method that makes the transition of the loading panel from transparent to opaque and vice versa when the scene finish loading
    /// </summary>
    /// <param name="canvasImage">Image, black image that fades and reappears</param>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator FadeCanvas(Image canvasImage)
    {
        // fade from transparent to opaque
        if (turnToBlack)
        {
            // loop over 1 second
            for (float currentFade = 0; currentFade <= 1; currentFade += Time.deltaTime)
            {
                // set color with currentFade as alpha
                canvasImage.color = new Color(0, 0, 0, currentFade);
                yield return null;
            }

            endFadingColor = true;
        }
        // fade from opaque to transparent
        else
        {
            // loop over 1 second backwards
            for (float currentFade = 1; currentFade >= 0; currentFade -= Time.deltaTime)
            {
                // set color with currentFade as alpha
                canvasImage.color = new Color(0, 0, 0, currentFade);
                yield return null;
            }

            endFadingColor = true;
        }
    }

    private void SetChanges(string currentInteractionObject)
    {
        int movementX;
        int movementY;

        Vector2 g_position = gameObject.transform.position;
        BoxCollider2D box = GetComponent<BoxCollider2D>();

        Vector3 box_center = g_position + box.offset;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = new Vector3(box_center.x, box_center.y, box_center.z);

        switch (currentInteractionObject)
        {
            case "BlackMarket_Door":
                movementX = 0;
                movementY = 1;

                GameObject warps = GameObject.Find("Warps");
                warps.transform.GetChild(0).gameObject.SetActive(true);
                warps.transform.GetChild(1).gameObject.SetActive(true);
                GameObject.Find("BlackMarket_Door").gameObject.transform.GetChild(1).gameObject.SetActive(true);

                Player_Objects.Small_Key = "used";
                break;

            case "Sword_Chest":
                movementX = 0;
                movementY = 1;

                Sprite[] gameObjects = Resources.LoadAll<Sprite>("Sprites/objects/objects");
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().sprite = gameObjects[6];

                Player_Objects.Chest_Key = "used";
                break;

            case "VillageWall_Knight":
                foreach (GameObject currentVillager in interactedVillagers)
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

                movementX = 0;
                movementY = 1;

                GameObject spikes = GameObject.Find("Spikes").gameObject;
                spikes.transform.GetChild(0).gameObject.SetActive(false);
                spikes.transform.GetChild(1).gameObject.SetActive(true);

                Player_Objects.Sword = "used";
                break;

            default:
                movementX = 0;
                movementY = 1;
                break;
        }

        player.GetComponent<Animator>().SetFloat("movementX", movementX);
        player.GetComponent<Animator>().SetFloat("movementY", movementY);

        hasCharged = true;
    }

    private void AdditionalChanges(string currentInteractionObject, bool reproduceSound)
    {
        switch (currentInteractionObject)
        {
            case "BlackMarket_Door":
                if (reproduceSound)
                {
                    objectSound = Resources.Load<AudioClip>("Sounds/Objects Sounds/lock open");
                }
                else
                {
                    GameObject.Find("BlackMarket_Door").gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    soundManager.GetComponent<SoundManager>().manageBackgroundMusicVolume("Music", PlayerPrefs.GetFloat("NormalBackgroundVolume"));
                    MultipleResources.PlayerIsTalking_or_isReading(false);
                }
                break;

            case "Sword_Chest":
                if (reproduceSound)
                {
                    objectSound = Resources.Load<AudioClip>("Sounds/Objects Sounds/lock open");
                }
                else
                {
                    GameObject.Find("Collectable").GetComponent<CollectablesInteraction>().StartPickUp("Sword");
                }
                break;

            case "VillageWall_Knight":
                if (reproduceSound)
                {
                    objectSound = Resources.Load<AudioClip>("Sounds/Objects Sounds/gate opening");
                    soundManager.GetComponent<SoundManager>().manageBackgroundMusicVolume("Music", PlayerPrefs.GetFloat("NormalBackgroundVolume"));
                }
                else
                {
                    MultipleResources.PlayerIsTalking_or_isReading(false);
                }
                break;
        }
    }

    public void SetObjectsSoundVolume()
    {
        objectVolume = (PlayerPrefs.GetFloat("SoundsVolume") * 0.01f); // 1 sound volume at max value
    }

    private IEnumerator SaveGame()
    {
        GameObject.Find("InventoryManager").GetComponent<InventoryDataRecoverer>().SaveProgress();

        while (GameObject.Find("InventoryManager") != null)
        {
            yield return null;
        }

        GameObject inventoryManager = new GameObject("InventoryManager");
        inventoryManager.AddComponent<InventoryDataRecoverer>();
        DontDestroyOnLoad(inventoryManager);

        SetObjectsSoundVolume();
        soundManager.GetComponent<SoundManager>().PlaySoundClip("ObjectInteraction", Resources.Load<AudioClip>("Sounds/Objects Sounds/save sound"), MultipleResources.PlayerPosition(), false, objectVolume);

        List<string> objectPhrases = RecoverPhrases("SaveGame");
        GameObject.Find("Collectable_Interaction_TextManager").GetComponent<Collectables_Objects_TextManager>().StartSaveText(objectPhrases, false, this);
    }
}