using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage all the music and sounds of the game
/// </summary>
public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// Create a GameObject with an audio source and the play the audio source
    /// </summary>
    /// <param name="gameObjectName">String, GameObject name</param>
    /// <param name="audio">AudioClip, clip that will be played</param>
    /// <param name="soundPosition">Vector3, Position where the sound is played</param>
    /// <param name="loop">Bool, true if the sound will loop, false if not</param>
    /// <param name="volume">Float, initial volume</param>
    /// <returns>AudioSource that is playing the sound</returns>
    public AudioSource PlaySoundClip(string gameObjectName, AudioClip audio, Vector3 soundPosition, bool loop, float volume)
    {
        GameObject soundGameObject = new GameObject(gameObjectName); // create the soundGameObject object
        soundGameObject.transform.position = soundPosition; // set its position
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>(); // add an audio source
        audioSource.clip = audio; // define the clip

        audioSource.loop = loop; // set if the sound will loop
        audioSource.volume = volume; // set the sound volume

        audioSource.Play(); // start the sound

        if (!audioSource.loop)
        {
            Destroy(soundGameObject, audio.length); // destroy object after clip duration
        }
        return audioSource; // return the AudioSource reference
    }

    /// <summary>
    /// Change the volume of an existing sound
    /// </summary>
    /// <param name="gameObjectName">String, AudioSource name that is reproducing the sound</param>
    /// <param name="volume">Float, new volume</param>
    public void manageBackgroundMusicVolume(string gameObjectName, float volume)
    {
        GameObject soundGameObject = GameObject.Find(gameObjectName);
        soundGameObject.GetComponent<AudioSource>().volume = volume;
    }

    /// <summary>
    /// Change the music
    /// </summary>
    /// <param name="gameObjectName">String, AudioSource name that is reproducing the music</param>
    /// <param name="audio">AudioClip, clip that will be played</param>
    /// <param name="volume">Float, initial volume</param>
    public void manageBackgroundMusic(string gameObjectName, AudioClip audio, float volume)
    {
        GameObject soundGameObject = GameObject.Find(gameObjectName);
        soundGameObject.GetComponent<AudioSource>().clip = audio;
        soundGameObject.GetComponent<AudioSource>().volume = volume;
        soundGameObject.GetComponent<AudioSource>().Play();
    }
}
