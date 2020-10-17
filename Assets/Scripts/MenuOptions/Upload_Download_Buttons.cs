using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upload_Download_Buttons : MonoBehaviour
{
    private GameObject databaseLabel;
    private bool operationInProgress;
    private bool closeDatabasePanel;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        databaseLabel = GameObject.Find("DatabaseLabel");
    }

    public void StartDatabaseProgress(string[] languageText)
    {
        operationInProgress = true;
        closeDatabasePanel = false;

        databaseLabel.GetComponent<TMPro.TextMeshProUGUI>().text = languageText[0];

        currentCoroutine = StartCoroutine(WritePoints(languageText));
    }

    public void CloseDatabasePanel()
    {
        closeDatabasePanel = true;
    }

    private IEnumerator WritePoints(string[] languageText)
    {
        int counter = 0;
        while (true)
        {
            if (operationInProgress)
            {
                if (counter >= 4)
                {
                    databaseLabel.GetComponent<TMPro.TextMeshProUGUI>().text = languageText[0];
                    counter = 0;
                    operationInProgress = false; // Quitar cuando se implemete el método de SQL
                }
                else
                {
                    databaseLabel.GetComponent<TMPro.TextMeshProUGUI>().text += " .";
                    counter++;
                }
            }
            else {
                if (!closeDatabasePanel)
                {
                    databaseLabel.GetComponent<TMPro.TextMeshProUGUI>().text = languageText[1];
                    GameObject.Find("DatabasePanel").transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = languageText[2];
                    GameObject.Find("DatabasePanel").transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    GameObject.Find("DatabasePanel").transform.GetChild(1).gameObject.SetActive(false);
                    GameObject.Find("MainMenu").transform.GetChild(1).gameObject.SetActive(false);
                    StopCoroutine(currentCoroutine);
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
