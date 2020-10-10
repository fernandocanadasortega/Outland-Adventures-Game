using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage the credits panel position and scrolling
/// </summary>
public class CreditsManager : MonoBehaviour
{
    private float startCreditsPositionY = -1662;
    private float endCreditsPositionY = 2557;
    private float currentPosition;
    private bool creditsON;

    /// <summary>
    /// Put the credits panel in the start position and start scrolling the credits
    /// </summary>
    public void StartCredits()
    {
        transform.position = new Vector3(transform.position.x, startCreditsPositionY, transform.position.z);
        this.gameObject.SetActive(true);
        currentPosition = startCreditsPositionY;
        creditsON = true;
        StartCoroutine(CreditsMovement());
        StartCoroutine(SkipCredits());
    }

    /// <summary>
    /// Scroll down the credits panel, if the credits are over return to the main menu
    /// </summary>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator CreditsMovement()
    {
        yield return null;

        while (currentPosition < endCreditsPositionY)
        {
            // Enter if you skip the credits
            if (!creditsON)
            {
                creditsON = false;
                yield break;
            }

            currentPosition++;
            transform.position = new Vector3(transform.position.x, currentPosition, transform.position.z);
            yield return null;
        }

        // Credits are over
        this.gameObject.SetActive(false);
        transform.parent.transform.GetChild(0).gameObject.SetActive(true);
        transform.parent.transform.GetChild(1).gameObject.SetActive(true);
        transform.parent.transform.GetChild(2).gameObject.SetActive(true);
        creditsON = false;
    }

    /// <summary>
    /// Check if the player press ESC, if pressed then return to the main menu
    /// </summary>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator SkipCredits()
    {
        yield return null;
        while (true)
        {
            if (Input.GetButton("Cancel")) 
            {
                this.gameObject.SetActive(false);
                transform.parent.transform.GetChild(0).gameObject.SetActive(true);
                transform.parent.transform.GetChild(1).gameObject.SetActive(true);
                transform.parent.transform.GetChild(2).gameObject.SetActive(true);
                creditsON = false;
            }

            if (!creditsON)
            {
                creditsON = false;
                yield break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// Open on internet the link clicked
    /// </summary>
    /// <param name="triggeredLink">URL of the clicked link</param>
    public void LinkClicked(GameObject triggeredLink)
    {
        System.Diagnostics.Process.Start(triggeredLink.GetComponent<TMPro.TextMeshProUGUI>().text);
    }
}