using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_DialogueManager : MonoBehaviour
{
    private Queue<string> senteces;
    public bool isTalking;
    private bool badConversation;
    private bool endSellerConversation;
    private bool writeCompletePhrase;
    private bool automaticWrite;
    private bool isSeller;
    private bool isTalkingToSeller;
    private VillagerInteraction villagerInteraction;
    private GuardsInteraction guardsInteraction;
    private NPC_DialogueSelector dialogue;

    /**
     * First element [0]: Panel with chat text and name
     * Second element [1]: Shop Panel  
     */
    private GameObject[] conversationPanels;

    /**
     *  First element [0]: villagerName  
     *  Second element [1]: villagerText  
     *  Third element [2]: nextPhrase  
     *  Fourth element [3]: skipConversation  
     *  fifth element [4]: autoConversation 
     *  Sixth element [5]: Buy Button (only with sellers)
     *  Seventh element [6]: Talk Button (only with sellers)
     *  Eighth element [7]: Exit Button (only with sellers)
     */
    private List<GameObject> conversationItems;

    private GameObject talkingCloud;
    public Animator boxAnimation;
    private GameObject soundManager;
    private AudioClip talkingSound;
    private float talkingVolume;


    // Start is called before the first frame update
    void Start()
    {
        conversationPanels = new GameObject[2];
        conversationItems = new List<GameObject>();
        senteces = new Queue<string>();
        soundManager = GameObject.Find("SoundManager");
        talkingSound = Resources.Load<AudioClip>("Sounds/Objects Sounds/npc talking");

        GameObject canvas = GameObject.Find("InformationCanvas");

        conversationPanels[0] = canvas.transform.GetChild(0).gameObject;
        for (int currentChild = 0; currentChild < conversationPanels[0].transform.childCount; currentChild++)
        {
            conversationItems.Add(conversationPanels[0].transform.GetChild(currentChild).gameObject);
        }

        conversationPanels[1] = canvas.transform.GetChild(1).gameObject;
        for (int currentChild = 0; currentChild < conversationPanels[1].transform.childCount; currentChild++)
        {
            conversationItems.Add(conversationPanels[1].transform.GetChild(currentChild).gameObject);
        }
    }

    public void StartConversation(NPC_DialogueSelector dialogue, GameObject talkingCloud, bool isSeller, VillagerInteraction villagerInteraction, GuardsInteraction guardsInteraction)
    {
        this.villagerInteraction = villagerInteraction;
        this.guardsInteraction = guardsInteraction;
        this.dialogue = dialogue;
        this.isSeller = isSeller;
        conversationPanels[0].SetActive(true);
        badConversation = false;
        endSellerConversation = false;
        setTalkingSoundVolume();

        soundManager.GetComponent<SoundManager>().manageBackgroundMusicVolume("Music", PlayerPrefs.GetFloat("ReducedBackgroundVolume"));

        if (automaticWrite)
        {
            On_Off_AutoMode();
        }

        isTalkingToSeller = false;
        conversationItems[1].GetComponent<Text>().color = Color.white;
        boxAnimation.SetBool("isTalking", true);
        isTalking = true;

        conversationItems[0].SetActive(true);
        conversationItems[1].SetActive(true);

        if (!isSeller)
        {
            EnableChatMenu();
        }
        
        if (talkingCloud != null)
        {
            this.talkingCloud = talkingCloud;
            talkingCloud.SetActive(true);
        }

        conversationItems[0].GetComponent<Text>().text = dialogue.npcName;

        senteces.Clear();

        EnqueueSentences();

        ReadSentence();
    }

    private void EnqueueSentences()
    {
        foreach (string currentSentence in dialogue.CurrentSentences)
        {
            senteces.Enqueue(currentSentence);
        }
    }

    public void ReadSentence()
    {
        writeCompletePhrase = false;
        conversationItems[2].SetActive(false);

        if (senteces.Count == 0 && !isTalkingToSeller)
        {
            EndConversation();
            return;
        }
        else if (senteces.Count == 0 && isTalkingToSeller)
        {
            returnToSellerMenu();
        }
        else
        {
            string currentSentence = senteces.Dequeue();

            if (!badConversation)
            {
                writeCompletePhrase = false;
                StartCoroutine("WriteLetters", currentSentence);
            }
        }
    }

    private IEnumerator WriteLetters(string sentence)
    {
        conversationItems[1].GetComponent<Text>().text = "";

        foreach (char currentLetter in sentence.ToCharArray())
        {
            conversationItems[1].GetComponent<Text>().text += currentLetter;

            if (!writeCompletePhrase)
            {
                if (currentLetter == 46)
                {
                    yield return new WaitForSeconds(1);
                }

                if (currentLetter == 44 || currentLetter == 58)
                {
                    yield return new WaitForSeconds(0.6f);
                }

                if (currentLetter != 32 && currentLetter != 13)
                {
                    soundManager.GetComponent<SoundManager>().PlaySoundClip("TextSound", talkingSound, MultipleResources.PlayerPosition(), false, talkingVolume); // 1 sound volume at max value
                }

                yield return new WaitForSeconds(0.05f);
            }
        }

        if (!badConversation && !automaticWrite && !endSellerConversation)
        {
            if (!isSeller || isTalkingToSeller)
            {
                conversationItems[2].SetActive(true);
            }
            else
            {
                EnableShopMenu();
            }
        }
        else if (automaticWrite && !badConversation)
        {
            yield return new WaitForSeconds(1f);

            conversationItems[2].SetActive(false);
            ReadSentence();
        }
        else if (badConversation || endSellerConversation)
        {
            yield return new WaitForSeconds(0.5f);

            if (!isTalkingToSeller)
            {
                EndConversation();
                yield break;
            }
            else
            {
                returnToSellerMenu();
            }
        }
    }

    public void setTalkingSoundVolume()
    {
        talkingVolume = (PlayerPrefs.GetFloat("SoundsVolume") * 0.01f);
    }

    public void CompleteCurrentPhrase()
    {
        if (!writeCompletePhrase)
        {
            writeCompletePhrase = true;
        }
    }

    public void On_Off_AutoMode()
    {
        if (automaticWrite)
        {
            automaticWrite = false;
            conversationItems[4].GetComponent<UnityEngine.UI.Image>().color = Color.HSVToRGB(1f, 0f, 1f);
        }
        else if (!automaticWrite)
        {
            automaticWrite = true;
            conversationItems[4].GetComponent<UnityEngine.UI.Image>().color = Color.HSVToRGB(0.38f, 0.7f, 1f);
        }
    }

    private void EnableChatMenu()
    {
        conversationItems[3].SetActive(true);
        conversationItems[4].SetActive(true);
    }

    private void DisableChatMenu()
    {
        conversationItems[3].SetActive(false);
        conversationItems[4].SetActive(false);
    }

    private void EnableShopMenu()
    {
        conversationPanels[1].SetActive(true);

        conversationItems[5].SetActive(true);
        conversationItems[6].SetActive(true);
        conversationItems[7].SetActive(true);

        conversationItems[5].GetComponent<Animator>().SetBool("isChoosing", true);
        conversationItems[6].GetComponent<Animator>().SetBool("isChoosing", true);
        conversationItems[7].GetComponent<Animator>().SetBool("isChoosing", true);
    }

    private void DisableShopMenu()
    {
        conversationItems[5].SetActive(false);
        conversationItems[6].SetActive(false);
        conversationItems[7].SetActive(false);

        conversationItems[5].GetComponent<Animator>().SetBool("isChoosing", false);
        conversationItems[6].GetComponent<Animator>().SetBool("isChoosing", false);
        conversationItems[7].GetComponent<Animator>().SetBool("isChoosing", false);

        conversationPanels[1].SetActive(false);
    }

    public void buyItemsSeller()
    {
        // change this method when buy interface is developed
        DisableShopMenu();
        EnableChatMenu();
        isTalkingToSeller = true;
        writeCompletePhrase = false;

        if (automaticWrite)
        {
            On_Off_AutoMode();
        }

        conversationItems[1].GetComponent<Text>().text = "";
        StopAllCoroutines();

        dialogue.NumberOfSentences = 1;
        dialogue.npcState = "Buy";
        dialogue.selectNewSentences();
        EnqueueSentences();
        StartCoroutine("WriteLetters", senteces.Dequeue());
    }

    public void talkSeller()
    {
        writeCompletePhrase = false;
        DisableShopMenu();
        EnableChatMenu();
        isTalkingToSeller = true;

        if (automaticWrite)
        {
            On_Off_AutoMode();
        }

        conversationItems[1].GetComponent<Text>().text = "";
        StopAllCoroutines();

        dialogue.NumberOfSentences = 1;
        dialogue.npcState = "Talk";
        dialogue.selectNewSentences();
        EnqueueSentences();
        StartCoroutine("WriteLetters", senteces.Dequeue());
    }

    private void returnToSellerMenu()
    {
        DisableChatMenu();
        isTalkingToSeller = false;
        writeCompletePhrase = false;
        badConversation = false;

        if (automaticWrite)
        {
            On_Off_AutoMode();
        }

        conversationItems[1].GetComponent<Text>().text = "";
        conversationItems[1].GetComponent<Text>().color = Color.white;
        StopAllCoroutines();

        dialogue.NumberOfSentences = 1;
        dialogue.npcState = "ReturnToMenu";
        dialogue.selectNewSentences();
        EnqueueSentences();
        StartCoroutine("WriteLetters", senteces.Dequeue());
    }

    public void EndSellerConversation()
    {
        endSellerConversation = true;
        writeCompletePhrase = false;
        DisableShopMenu();
        conversationItems[1].GetComponent<Text>().text = "";
        StopAllCoroutines();

        dialogue.NumberOfSentences = 1;
        dialogue.npcState = "Exit";
        dialogue.selectNewSentences();
        EnqueueSentences();
        StartCoroutine("WriteLetters", senteces.Dequeue());
    }

    public void EndRushedConversation()
    {
        senteces.Clear();
        badConversation = true;
        writeCompletePhrase = false;

        conversationItems[2].SetActive(false);
        DisableChatMenu();

        conversationItems[1].GetComponent<Text>().text = "";
        conversationItems[1].GetComponent<Text>().color = Color.red;

        StopAllCoroutines();

        string currentSentence;

        if (!isTalkingToSeller)
        {
             currentSentence = "DE ACUERDO . . .   ADIOS";
        }
        else
        {
            currentSentence = " . . . ";
        }

        StartCoroutine("WriteLetters", currentSentence);
    }

    private void EndConversation()
    {
        writeCompletePhrase = false;
        boxAnimation.SetBool("isTalking", false);

        if (talkingCloud != null)
        {
            talkingCloud.SetActive(false);
        }

        if (isSeller)
        {
            dialogue.npcState = "Welcome";
        }

        conversationItems[0].SetActive(false);
        conversationItems[1].SetActive(false);
        conversationItems[2].SetActive(false);
        DisableChatMenu();
        conversationPanels[0].SetActive(false);

        isTalking = false;

        if ((villagerInteraction != null && villagerInteraction.giveObject) || (guardsInteraction != null && guardsInteraction.giveObject))
        {
            try
            {
                GameObject.Find("Collectable").GetComponent<CollectablesInteraction>().StartPickUp(villagerInteraction.requiredObjectTag);
            }
            catch (System.Exception)
            {
                GameObject.Find("Collectable").GetComponent<CollectablesInteraction>().StartPickUp(guardsInteraction.requiredObjectTag);
            }
        }

        else if ((villagerInteraction != null && villagerInteraction.needObject) || (guardsInteraction != null && guardsInteraction.needObject))
        {
            try
            {
                GameObject.Find("Object_To_Interact").GetComponent<ObjectsInteraction>().GetObject(villagerInteraction.requiredObjectTag);
            }
            catch (System.Exception)
            {
                GameObject.Find("Object_To_Interact").GetComponent<ObjectsInteraction>().GetObject(guardsInteraction.requiredObjectTag);
            }
        }

        else
        {
            MultipleResources.PlayerIsTalking_or_isReading(false);
            soundManager.GetComponent<SoundManager>().manageBackgroundMusicVolume("Music", PlayerPrefs.GetFloat("NormalBackgroundVolume"));
        }

        try
        {
            GameObject.Find("Villager").GetComponent<VillagerMovement>().isTalking = false;

        }
        catch (System.Exception)
        { }   
    }
}