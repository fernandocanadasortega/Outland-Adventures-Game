using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerInteraction : MonoBehaviour
{
    public GameObject talkingCloud;
    public bool giveObject; // to player
    public bool needObject; // from player
    public string requiredObjectTag;
    private Vector3 villagerLastPosition = Vector3.zero;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            villagerLastPosition = Vector3.zero;
            GetComponent<VillagerMovement>().startTalking();
        }
        else if (collision.tag == "Villager")
        {
            if (!collision.GetComponent<VillagerMovement>().isTalking && !collision.GetComponent<VillagerMovement>().isAboutToTalk)
            {
                collision.GetComponent<VillagerMovement>().changeDirection();
            }

            if (!GetComponent<VillagerMovement>().isTalking && !GetComponent<VillagerMovement>().isAboutToTalk)
            {
                GetComponent<VillagerMovement>().changeDirection();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (villagerLastPosition != Vector3.zero && villagerLastPosition != transform.position)
            {
                GetComponent<VillagerMovement>().isPushed(true);
            }
            else
            {
                GetComponent<VillagerMovement>().isPushed(false);
            }

            villagerLastPosition = transform.position;

            Vector3 playerPosition = MultipleResources.PlayerPosition();

            float villagerYPosition = Mathf.Abs(transform.position.y);
            float playerYPosition = Mathf.Abs(playerPosition.y);

            // player over npc
            if ((villagerYPosition - playerYPosition) < 2.991388f && (villagerYPosition - playerYPosition) > 1.5f)
            {
                GetComponent<VillagerMovement>().talkingDirection = 0;
            }
            // npc under player
            else if ((playerYPosition - villagerYPosition) < 2.41297f && (playerYPosition - villagerYPosition) > 1.5f)
            {
                GetComponent<VillagerMovement>().talkingDirection = 2;
            }
            else
            {
                // player right to npc
                if (transform.position.x < playerPosition.x)
                {
                    GetComponent<VillagerMovement>().talkingDirection = 1;
                }
                // player left to npc
                else if (transform.position.x > playerPosition.x)
                {
                    GetComponent<VillagerMovement>().talkingDirection = 3;
                }
            }

            if (!GameObject.Find("NPC_DialogueManager").GetComponent<NPC_DialogueManager>().isTalking && Input.GetButton("Interaction") && !MultipleResources.PlayerIsTalking_or_isReading())
            {
                if (giveObject && !requiredObjectTag.Equals(""))
                {
                    MultipleResources.PlayerIsTalking_or_isReading(true);
                    GetComponent<NPC_DialogueSelector>().NumberOfSentences = 1;
                    GetComponent<NPC_DialogueSelector>().selectNewSentences();
                    GameObject.Find("NPC_DialogueManager").GetComponent<NPC_DialogueManager>().StartConversation(GetComponent<NPC_DialogueSelector>(), talkingCloud, false, this, null);
                }
                else if (needObject && !requiredObjectTag.Equals(""))
                {
                    MultipleResources.PlayerIsTalking_or_isReading(true);
                    GetComponent<NPC_DialogueSelector>().NumberOfSentences = 1;
                    GetComponent<NPC_DialogueSelector>().selectNewSentences();
                    GameObject.Find("NPC_DialogueManager").GetComponent<NPC_DialogueManager>().StartConversation(GetComponent<NPC_DialogueSelector>(), talkingCloud, false, this, null);
                }
                else
                {
                    GetComponent<NPC_DialogueSelector>().NumberOfSentences = 1;
                    GetComponent<NPC_DialogueSelector>().selectNewSentences();
                    MultipleResources.PlayerIsTalking_or_isReading(true);
                    GameObject.Find("NPC_DialogueManager").GetComponent<NPC_DialogueManager>().StartConversation(GetComponent<NPC_DialogueSelector>(), talkingCloud, false, this, null);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetComponent<VillagerMovement>().isAboutToTalk = false;
        }
    }
}
