using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of set the camera movement boundaries
/// </summary>
public class Collectables_Objects_TextManager : MonoBehaviour
{
    private Queue<string> senteces;
    public Animator boxAnimation;
    private AudioClip talkingSound;
    private GameObject soundManager;
    private GameObject messagePanel;
    /**
    *  First element [0]: messageText  
    *  Second element [1]: Yes_Option
    *  Third element [2]: No_Option  
    */
    private List<GameObject> messageItems;

    private float timeReadingPause = 0.9f;
    private string hasItem;
    private bool isCollectable;
    private bool gonnaSave;
    private ObjectsInteraction objectsInteraction;
    private float lettersVolume;

    private void Start()
    {
        senteces = new Queue<string>();
        soundManager = GameObject.Find("SoundManager");
        messageItems = new List<GameObject>();
        talkingSound = Resources.Load<AudioClip>("Sounds/Objects Sounds/npc talking");

        GameObject canvas = GameObject.Find("InformationCanvas");

        try
        {
            messagePanel = canvas.transform.GetChild(2).gameObject;
        }
        catch (System.Exception)
        {
            try
            {
                messagePanel = canvas.transform.GetChild(1).gameObject;
            }
            catch (System.Exception)
            {
                messagePanel = canvas.transform.GetChild(0).gameObject;
            }
        }

        for (int currentChild = 0; currentChild < messagePanel.transform.childCount; currentChild++)
        {
            messageItems.Add(messagePanel.transform.GetChild(currentChild).gameObject);
        }
    }

    public void StartObjectInteractionText(List<string> objectPhrases, string hasItem, ObjectsInteraction objectsInteraction)
    {
        this.objectsInteraction = objectsInteraction;
        this.hasItem = hasItem;
        isCollectable = false;
        messagePanel.SetActive(true);
        boxAnimation.SetBool("isReadingText", true);

        messageItems[0].GetComponent<Text>().text = "";
        messageItems[0].SetActive(true);

        senteces.Clear();
        SetLettersSoundVolume();

        if (hasItem.Equals("none"))
        {
            EnqueueSentences(objectPhrases, "", 1);
        }
        else
        {
            timeReadingPause = 1.4f;
            EnqueueSentences(objectPhrases, "", 2);
            messageItems[1].GetComponent<Button>().transform.GetChild(0).gameObject.GetComponent<Text>().text = objectPhrases[2];
            messageItems[2].GetComponent<Button>().transform.GetChild(0).gameObject.GetComponent<Text>().text = objectPhrases[3];
        }

        ReadSentence();
    }

    public void StartCollectableInteractionText(string collectablePhrase)
    {
        isCollectable = true;
        messagePanel.SetActive(true);
        boxAnimation.SetBool("isReadingText", true);

        messageItems[0].GetComponent<Text>().text = "";
        messageItems[0].SetActive(true);

        senteces.Clear();
        SetLettersSoundVolume();

        EnqueueSentences(null, collectablePhrase, 1);

        ReadSentence();
    }

    public void StartSaveText(List<string> objectPhrases, bool gonnaSave, ObjectsInteraction objectsInteraction)
    {
        this.objectsInteraction = objectsInteraction;
        isCollectable = false;
        this.gonnaSave = gonnaSave;
        messagePanel.SetActive(true);
        hasItem = "";
        boxAnimation.SetBool("isReadingText", true);

        messageItems[0].GetComponent<Text>().text = "";
        messageItems[0].SetActive(true);

        senteces.Clear();
        SetLettersSoundVolume();

        if (gonnaSave)
        {
            EnqueueSentences(objectPhrases, "", 1);
            messageItems[1].GetComponent<Button>().transform.GetChild(0).gameObject.GetComponent<Text>().text = objectPhrases[2];
            messageItems[2].GetComponent<Button>().transform.GetChild(0).gameObject.GetComponent<Text>().text = objectPhrases[3];
        }
        else
        {
            senteces.Enqueue(objectPhrases[1]);
        }

        ReadSentence();
    }

    private void EnqueueSentences(List<string> objectPhrases, string collectablePhrase, int numberOfSentences)
    {
        if (!isCollectable && objectPhrases != null)
        {
            for (int numerOfIteration = 0; numerOfIteration < numberOfSentences; numerOfIteration++)
            {
                senteces.Enqueue(objectPhrases[numerOfIteration]);
            }
        }
        else if (isCollectable && !collectablePhrase.Equals(""))
        {
            senteces.Enqueue(collectablePhrase);
        }
    }

    private void ReadSentence()
    {
        if (senteces.Count == 0)
        {
            try
            {
                if ((hasItem.Equals("none") && !isCollectable) || isCollectable)
                {
                    EndConversation();
                    RestoreMusic();
                    return;
                }
                else if (hasItem.Equals("got") && !isCollectable)
                {
                    ShowOptions();
                }
                else if (gonnaSave)
                {
                    ShowOptions();
                }
                else if (!gonnaSave)
                {
                    EndConversation();
                    RestoreMusic();
                    return;
                }
            }
            catch (System.Exception)
            {
                EndConversation();
                RestoreMusic();
                return;
            }
        }
        else
        {
            string currentSentence = senteces.Dequeue();

            StartCoroutine(WriteLetters(currentSentence));
        }
    }

    private IEnumerator WriteLetters(string sentence)
    {
        messageItems[0].GetComponent<Text>().text = "";

        foreach (char currentLetter in sentence.ToCharArray())
        {
            messageItems[0].GetComponent<Text>().text += currentLetter;

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
                soundManager.GetComponent<SoundManager>().PlaySoundClip("TextSound", talkingSound, MultipleResources.PlayerPosition(), false, lettersVolume); // 1 sound volume at max value
            }

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(timeReadingPause);
        ReadSentence();
    }

    public void SetLettersSoundVolume()
    {
        lettersVolume = (PlayerPrefs.GetFloat("SoundsVolume") * 0.01f);
    }

    private void ShowOptions()
    {
        Vector3 textPosition = messageItems[0].transform.position;
        messageItems[0].transform.position = new Vector3(textPosition.x, textPosition.y + 17f, textPosition.z);

        messageItems[1].SetActive(true);
        messageItems[2].SetActive(true);
    }

    public void Yes_Option()
    {
        EndConversation();
        objectsInteraction.StartChanges();
        RestoreTextPosition();
        return;
    }

    public void No_Option()
    {
        EndConversation();
        RestoreTextPosition();
        RestoreMusic();
        return;
    }

    private void RestoreTextPosition()
    {
        Vector3 textPosition = messageItems[0].transform.position;
        messageItems[0].transform.position = new Vector3(textPosition.x, textPosition.y - 17f, textPosition.z);
    }

    private void EndConversation()
    {
        boxAnimation.SetBool("isReadingText", false);

        messageItems[0].SetActive(false);
        messageItems[1].SetActive(false);
        messageItems[2].SetActive(false);

        messagePanel.SetActive(false);

        MultipleResources.PlayerIsTalking_or_isReading(false);
    }

    private void RestoreMusic()
    {
        soundManager.GetComponent<SoundManager>().manageBackgroundMusicVolume("Music", PlayerPrefs.GetFloat("NormalBackgroundVolume"));
    }
}