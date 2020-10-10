using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge change the sprites from close to open door and vice versa and play the corresponding sound
/// </summary>
public class Doors_Triggers : MonoBehaviour
{
    public Sprite closedDoor, openedDoor;
    public GameObject door;
    private GameObject soundManager;
    private AudioClip doorOpening, doorClosing;
    private float doorVolume;

    /// <summary>
    /// Function that is called right after the scene is loaded and search the sounds of the door opening and closing
    /// </summary>
    private void Awake()
    {
        soundManager = GameObject.Find("SoundManager");

        doorOpening = Resources.Load<AudioClip>("Sounds/Objects Sounds/Door Opening");
        doorClosing = Resources.Load<AudioClip>("Sounds/Objects Sounds/Door Closing");
    }

    /// <summary>
    /// Change the door sprite from closed door to opened door and reproduce the sound of the door opening
    /// </summary>
    /// <param name="collision">Gameobject that enter the door area</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            try
            {
                door.GetComponent<SpriteRenderer>().sprite = openedDoor;
                setDoorSoundVolume();
                soundManager.GetComponent<SoundManager>().PlaySoundClip("DoorSound", doorOpening, MultipleResources.PlayerPosition(), false, doorVolume);
            }
            catch (System.Exception)
            {
                door.GetComponent<SpriteRenderer>().sprite = closedDoor;
            }
        }
    }

    /// <summary>
    /// Change the door sprite from opened door to closed door and reproduce the sound of the door closing
    /// </summary>
    /// <param name="collision">Gameobject that enter the door area</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            try
            {
                door.GetComponent<SpriteRenderer>().sprite = closedDoor;
                setDoorSoundVolume();
                soundManager.GetComponent<SoundManager>().PlaySoundClip("DoorSound", doorClosing, MultipleResources.PlayerPosition(), false, doorVolume);
            }
            catch (System.Exception)
            {
                door.GetComponent<SpriteRenderer>().sprite = closedDoor;
            }
        }
    }

    /// <summary>
    /// Set the door sound volume
    /// </summary>
    public void setDoorSoundVolume()
    {
        doorVolume = (PlayerPrefs.GetFloat("SoundsVolume") / 125);
    }
}
