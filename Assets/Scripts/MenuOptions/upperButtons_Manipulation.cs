using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage the movement and interaction of the buttons and text in the upper side of the main menu scene
/// </summary>
public class upperButtons_Manipulation : MonoBehaviour
{
    private GameObject upperButtons;
    private List<GameObject> textLabels, buttonPanels, buttons;

    private int itemsCount = 4;
    private bool showingButton, showingText;

    /// <summary>
    /// Function that is called right after the scene is loaded and get from the main menu scene all the GameObjects, such as the buttons, texts and panels and save it into List
    /// </summary>
    private void Awake()
    {
        upperButtons = this.gameObject;
        textLabels = new List<GameObject>();
        buttonPanels = new List<GameObject>();
        buttons = new List<GameObject>();

        int iterationNumber = 0;
        iterationNumber += itemsCount; 

        for (int currentGameObject = 0; currentGameObject < iterationNumber; currentGameObject++)
        {
            textLabels.Add(upperButtons.transform.GetChild(currentGameObject).gameObject);
        }

        iterationNumber += itemsCount;
        for (int currentGameObject = 4; currentGameObject < iterationNumber; currentGameObject++)
        {
            buttonPanels.Add(upperButtons.transform.GetChild(currentGameObject).gameObject);
        }

        iterationNumber += itemsCount;
        for (int currentGameObject = 8; currentGameObject < iterationNumber; currentGameObject++)
        {
            buttons.Add(upperButtons.transform.GetChild(currentGameObject).gameObject);
        }
    }

    /// <summary>
    /// Move the colliding button if the is no space to the text to show correctly (Move the button "load progress" when the mouse enter the button "save progress")
    /// </summary>
    /// <param name="isOpening">true if the mouse enter the button, false if the mouse leave the button</param>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator Button_Panel_PositiveMovement(bool isOpening)
    {
        yield return null;

        float buttonPosition = buttons[1].transform.position.x;
        float transitionVelocity = 30;

        if (isOpening) // go movement
        {
            float distance = buttonPosition + textLabels[0].GetComponent<RectTransform>().rect.width - 30;

            for (float buttonMovementCounter = buttonPosition; buttonMovementCounter < distance; buttonMovementCounter += transitionVelocity)
            {
                buttons[1].transform.position = new Vector3(buttonMovementCounter, buttons[1].transform.position.y, buttons[1].transform.position.z);
                buttonPanels[1].transform.position = new Vector3(buttonMovementCounter, buttonPanels[1].transform.position.y, buttonPanels[1].transform.position.z);
                yield return null;
            }
        }
        else // return movement
        {
            float distance = buttonPosition - textLabels[0].GetComponent<RectTransform>().rect.width + 30;

            for (float buttonMovementCounter = buttonPosition; buttonMovementCounter > distance; buttonMovementCounter -= transitionVelocity)
            {
                buttons[1].transform.position = new Vector3(buttonMovementCounter, buttons[1].transform.position.y, buttons[1].transform.position.z);
                buttonPanels[1].transform.position = new Vector3(buttonMovementCounter, buttonPanels[1].transform.position.y, buttonPanels[1].transform.position.z);
                yield return null;
            }
        }
        showingButton = false;
    }

    /// <summary>
    /// Show the text of the button the mouse is in (When the text appears to the right of the button)
    /// </summary>
    /// <param name="isShowing">True is the text is appearing, false if the text is disappearing</param>
    /// <param name="objectIndex">The button index the text is associated to</param>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator TextLabel_PositiveMovement(bool isShowing, int objectIndex)
    {
        yield return null;
        float textPosition = textLabels[objectIndex].transform.position.x;
        float transitionVelocity = 10;

        if (isShowing) // go movement
        {
            float distance = textPosition + buttons[objectIndex].GetComponent<RectTransform>().rect.width;

            for (float textMovementCounter = textPosition; textMovementCounter < distance; textMovementCounter += transitionVelocity)
            {
                textLabels[objectIndex].transform.position = new Vector3(textMovementCounter, textLabels[objectIndex].transform.position.y, textLabels[objectIndex].transform.position.z);
                yield return null;
            }
        }
        else // return movement
        {
            float distance = textPosition - buttons[objectIndex].GetComponent<RectTransform>().rect.width;

            for (float textMovementCounter = textPosition; textMovementCounter > distance; textMovementCounter -= transitionVelocity)
            {
                textLabels[objectIndex].transform.position = new Vector3(textMovementCounter, textLabels[objectIndex].transform.position.y, textLabels[objectIndex].transform.position.z);
                yield return null;
            }

            textLabels[objectIndex].SetActive(false);
            buttonPanels[objectIndex].SetActive(false);
            if (objectIndex == 0)
            {
                buttonPanels[objectIndex + 1].SetActive(false);
            }
        }
        showingText = false;
    }

    /// <summary>
    /// Move the colliding button if the is no space to the text to show correctly (Move the button "configuration" when the mouse enter the button "log out")
    /// </summary>
    /// <param name="isOpening">true if the mouse enter the button, false if the mouse leave the button</param>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator Button_Panel_NegativeMovement(bool isOpening)
    {
        yield return null;
        float buttonPosition = buttons[2].transform.position.x;
        float transitionVelocity = 30;

        if (isOpening) // go movement
        {
            float distance = buttonPosition - textLabels[3].GetComponent<RectTransform>().rect.width + 30;

            for (float buttonMovementCounter = buttonPosition; buttonMovementCounter > distance; buttonMovementCounter -= transitionVelocity)
            {
                buttons[2].transform.position = new Vector3(buttonMovementCounter, buttons[2].transform.position.y, buttons[2].transform.position.z);
                buttonPanels[2].transform.position = new Vector3(buttonMovementCounter, buttonPanels[2].transform.position.y, buttonPanels[2].transform.position.z);
                yield return null;
            }
        }
        else // return movement
        {
            float distance = buttonPosition + textLabels[3].GetComponent<RectTransform>().rect.width -30;

            for (float buttonMovementCounter = buttonPosition; buttonMovementCounter < distance; buttonMovementCounter += transitionVelocity)
            {
                buttons[2].transform.position = new Vector3(buttonMovementCounter, buttons[2].transform.position.y, buttons[2].transform.position.z);
                buttonPanels[2].transform.position = new Vector3(buttonMovementCounter, buttonPanels[2].transform.position.y, buttonPanels[2].transform.position.z);
                yield return null;
            }
        }
        showingButton = false;
    }

    /// <summary>
    /// Show the text of the button the mouse is in (When the text appears to the left of the button)
    /// </summary>
    /// <param name="isShowing">True is the text is appearing, false if the text is disappearing</param>
    /// <param name="objectIndex">The button index the text is associated to</param>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator TextLabel_NegativeMovement(bool isShowing, int objectIndex)
    {
        yield return null;
        float textPosition = textLabels[objectIndex].transform.position.x;
        float transitionVelocity = 10;

        if (isShowing) // go movement
        {
            float distance = textPosition - buttons[objectIndex].GetComponent<RectTransform>().rect.width;

            for (float textMovementCounter = textPosition; textMovementCounter > distance; textMovementCounter -= transitionVelocity)
            {
                textLabels[objectIndex].transform.position = new Vector3(textMovementCounter, textLabels[objectIndex].transform.position.y, textLabels[objectIndex].transform.position.z);
                yield return null;
            }
        }
        else // return movement
        {
            float distance = textPosition + buttons[objectIndex].GetComponent<RectTransform>().rect.width;

            for (float textMovementCounter = textPosition; textMovementCounter < distance; textMovementCounter += transitionVelocity)
            {
                textLabels[objectIndex].transform.position = new Vector3(textMovementCounter, textLabels[objectIndex].transform.position.y, textLabels[objectIndex].transform.position.z);
                yield return null;
            }

            textLabels[objectIndex].SetActive(false);
            buttonPanels[objectIndex].SetActive(false);
            if (objectIndex == 3)
            {
                buttonPanels[objectIndex - 1].SetActive(false);
            }
        }
        showingText = false;
    }

    /// <summary>
    /// Check the multiple variables and call to different methods according to this variables
    /// </summary>
    /// <param name="positiveMovement">True if the text and button movement animation is towards right, false if the text and button movement animation is towards left</param>
    /// <param name="buttonAnimationTrigger">True to start the button movement animation (to move the colliding button if the is no space to the text to show correctly)</param>
    /// <param name="textAnimationTrigger">True to start the text movement animation</param>
    /// <param name="isOpening">true if the mouse enter the button, false if the mouse leave the button</param>
    /// <param name="isShowing">True is the text is appearing, false if the text is disappearing</param>
    /// <param name="objectIndex">The button index the text is associated to</param>
    /// <returns>Its does not return anything, but the couroutine use it to wait a specific time</returns>
    private IEnumerator statusChecker(bool positiveMovement, bool buttonAnimationTrigger, bool textAnimationTrigger, bool isOpening, bool isShowing, int objectIndex)
    {
        while (showingButton || showingText)
        {
            yield return null;
        }

        if (buttonAnimationTrigger && textAnimationTrigger)
        {
            showingButton = true;
            showingText = true;

            if (positiveMovement)
            {
                StartCoroutine(Button_Panel_PositiveMovement(isOpening));
                StartCoroutine(TextLabel_PositiveMovement(isShowing, objectIndex));
            }
            else
            {
                StartCoroutine(Button_Panel_NegativeMovement(isOpening));
                StartCoroutine(TextLabel_NegativeMovement(isShowing, objectIndex));
            }
        }

        else if (buttonAnimationTrigger && !textAnimationTrigger)
        {
            showingButton = true;

            if (positiveMovement)
            {
                StartCoroutine(Button_Panel_PositiveMovement(isOpening));
            }
            else
            {
                StartCoroutine(Button_Panel_NegativeMovement(isOpening));
            }
        }

        else if (!buttonAnimationTrigger && textAnimationTrigger)
        {
            showingText = true;

            if (positiveMovement)
            {
                StartCoroutine(TextLabel_PositiveMovement(isShowing, objectIndex));
            }
            else
            {
                StartCoroutine(TextLabel_NegativeMovement(isShowing, objectIndex));
            }
        }

    }

    /// <summary>
    /// This function is called when the mouse enter the "save progress" button, call StatusChecker and show the text corresponding to this button
    /// </summary>
    public void OnUploadEnter()
    {
        try
        {
            StartCoroutine(statusChecker(true, true, true, true, true, 0));
            buttons[0].GetComponent<Animator>().SetTrigger("Highlighted");

            buttonPanels[0].SetActive(true);
            buttonPanels[1].SetActive(true);
            textLabels[0].SetActive(true);
        }
        catch (System.Exception)
        { }
    }

    /// <summary>
    /// This function is called when the mouse exit the "save progress" button
    /// </summary>
    public void OnUploadExit()
    {
        try
        {
            StartCoroutine(statusChecker(true, true, true, false, false, 0));
            buttons[0].GetComponent<Animator>().SetTrigger("Normal");
        }
        catch (System.Exception)
        { }
    }

    /// <summary>
    /// Stop the button animation (not the movement one), and call the corresponding function when pressed
    /// </summary>
    public void OnUploadClick()
    {
        buttons[0].GetComponent<Animator>().SetTrigger("Pressed");
    }

    /// <summary>
    /// This function is called when the mouse enter the "load progress" button, call StatusChecker and show the text corresponding to this button
    /// </summary>
    public void OnDownloadEnter()
    {
        try
        {
            buttons[1].GetComponent<Animator>().SetTrigger("Highlighted");

            buttonPanels[1].SetActive(true);
            textLabels[1].SetActive(true);

            StartCoroutine(statusChecker(true, false, true, false, true, 1));
        }
        catch (System.Exception)
        { }
    }

    /// <summary>
    /// This function is called when the mouse exit the "load progress" button
    /// </summary>
    public void OnDownloadExit()
    {
        try
        {
            StartCoroutine(statusChecker(true, false, true, false, false, 1));

            buttons[1].GetComponent<Animator>().SetTrigger("Normal");
        }
        catch (System.Exception)
        { }
    }

    /// <summary>
    /// Stop the button animation (not the movement one), and call the corresponding function when pressed
    /// </summary>
    public void OnDownloadClick()
    {
        buttons[1].GetComponent<Animator>().SetTrigger("Pressed");
    }

    /// <summary>
    /// This function is called when the mouse enter the "configuration" button, call StatusChecker and show the text corresponding to this button
    /// </summary>
    public void OnConfigurationEnter()
    {
        try
        {
            buttons[2].GetComponent<Animator>().SetTrigger("Highlighted");

            buttonPanels[2].SetActive(true);
            textLabels[2].SetActive(true);

            StartCoroutine(statusChecker(false, false, true, false, true, 2));
        }
        catch (System.Exception)
        { }
    }

    /// <summary>
    /// This function is called when the mouse exit the "configuration" button
    /// </summary>
    public void OnConfigurationExit()
    {
        try
        {
            StartCoroutine(statusChecker(false, false, true, false, false, 2));

            buttons[2].GetComponent<Animator>().SetTrigger("Normal");
        }
        catch (System.Exception)
        { }
    }

    /// <summary>
    /// Stop the button animation (not the movement one), and call the corresponding function when pressed
    /// </summary>
    public void OnConfigurationClick()
    {
        buttons[2].GetComponent<Animator>().SetTrigger("Pressed");
        GameObject.Find("PauseCanvas").GetComponent<Pause_UI>().StartMenuPauseInteraction();
    }

    /// <summary>
    /// This function is called when the mouse enter the "log out" button, call StatusChecker and show the text corresponding to this button
    /// </summary>
    public void OnLogoutEnter()
    {
        try
        {
            buttons[3].GetComponent<Animator>().SetTrigger("Highlighted");

            buttonPanels[2].SetActive(true);
            buttonPanels[3].SetActive(true);
            textLabels[3].SetActive(true);

            StartCoroutine(statusChecker(false, true, true, true, true, 3));
        }
        catch (System.Exception)
        { }
    }

    /// <summary>
    /// This function is called when the mouse exit the "log out" button
    /// </summary>
    public void OnLogoutExit()
    {
        try
        {
            buttonPanels[3].SetActive(false);
            textLabels[3].SetActive(false);

            buttons[3].GetComponent<Animator>().SetTrigger("Normal");

        StartCoroutine(statusChecker(false, true, true, false, false, 3));
        }
        catch (System.Exception)
        { }
    }

    /// <summary>
    /// Stop the button animation (not the movement one), and call the corresponding function when pressed
    /// </summary>
    public void OnLogoutClick()
    {
        buttons[3].GetComponent<Animator>().SetTrigger("Pressed");
    }
}