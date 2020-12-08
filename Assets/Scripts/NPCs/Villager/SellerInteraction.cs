using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellerInteraction : MonoBehaviour
{
    public GameObject talkingCloud;

    /// <summary>
    /// Start seller interaction if the player isn't talking and the player pressed the interaction button
    /// </summary>
    /// <param name="collision">Gameobject that enter the seller collider area</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!GameObject.Find("NPC_DialogueManager").GetComponent<NPC_DialogueManager>().isTalking && Input.GetButton("Interaction") && !MultipleResources.PlayerIsTalking_or_isReading())
            {
                GetComponent<NPC_DialogueSelector>().NumberOfSentences = 1;
                GetComponent<NPC_DialogueSelector>().selectNewSentences();
                MultipleResources.PlayerIsTalking_or_isReading(true);
                GameObject.Find("NPC_DialogueManager").GetComponent<NPC_DialogueManager>().StartConversation(GetComponent<NPC_DialogueSelector>(), talkingCloud, true, null, null);
            }
        }
    }
}
