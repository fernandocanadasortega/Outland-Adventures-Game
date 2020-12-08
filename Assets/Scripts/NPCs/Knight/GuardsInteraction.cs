using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardsInteraction : MonoBehaviour
{
    public GameObject talkingCloud;
    public bool giveObject; // to player
    public bool needObject; // from player
    public string requiredObjectTag;

    /// <summary>
    /// Start guard interaction if the player isn't talking and the player pressed the interaction button, also check if the guard needs
    /// something from you or can give you something
    /// </summary>
    /// <param name="collision">Gameobject that enter the seller collider area</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!GameObject.Find("NPC_DialogueManager").GetComponent<NPC_DialogueManager>().isTalking && Input.GetButton("Interaction") && !MultipleResources.PlayerIsTalking_or_isReading())
            {
                if (giveObject && !requiredObjectTag.Equals(""))
                {
                    MultipleResources.PlayerIsTalking_or_isReading(true);
                    GetComponent<NPC_DialogueSelector>().NumberOfSentences = 1;
                    GetComponent<NPC_DialogueSelector>().selectNewSentences();
                    GameObject.Find("NPC_DialogueManager").GetComponent<NPC_DialogueManager>().StartConversation(GetComponent<NPC_DialogueSelector>(), talkingCloud, false, null, this);
                }
                else if (needObject && !requiredObjectTag.Equals(""))
                {
                    MultipleResources.PlayerIsTalking_or_isReading(true);
                    GetComponent<NPC_DialogueSelector>().NumberOfSentences = 1;
                    GetComponent<NPC_DialogueSelector>().selectNewSentences();
                    GameObject.Find("NPC_DialogueManager").GetComponent<NPC_DialogueManager>().StartConversation(GetComponent<NPC_DialogueSelector>(), talkingCloud, false, null, this);
                }
                else
                {
                    GetComponent<NPC_DialogueSelector>().NumberOfSentences = 1;
                    GetComponent<NPC_DialogueSelector>().selectNewSentences();
                    MultipleResources.PlayerIsTalking_or_isReading(true);
                    GameObject.Find("NPC_DialogueManager").GetComponent<NPC_DialogueManager>().StartConversation(GetComponent<NPC_DialogueSelector>(), talkingCloud, false, null, this);
                }
            }
        }
    }
}
