using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is composed of static methods and attributes that other classes use
/// </summary>
public class MultipleResources
{
    // Check if the music is already started or not
    public static bool musicStarted = false;

    /// <summary>
    /// Find the player GameObject and return its position
    /// </summary>
    /// <returns>Vector3, player position</returns>
    public static Vector3 PlayerPosition()
    {
        return GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    /// <summary>
    /// Find the player GameObject and return if the player is talking or reading
    /// </summary>
    /// <returns>Bool, true if the player is talking or reading, false if not</returns>
    public static bool PlayerIsTalking_or_isReading()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<player>().isTalking_or_isReading;
    }

    /// <summary>
    /// Find the player GameObject and change the talking_or_reading value
    /// </summary>
    /// <param name="isTalking_or_isReading">Bool, true if the player is talking or reading, false if not</param>
    public static void PlayerIsTalking_or_isReading(bool isTalking_or_isReading)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<player>().isTalking_or_isReading = isTalking_or_isReading;
    }
}
