using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that selects phrases from npc according to their function from the dictionary randomly
/// </summary>
[System.Serializable]
public class NPC_DialogueSelector : MonoBehaviour
{
    public string npcName;
    public string npcFunction;
    public string npcState;
    private int numberOfSentences = 2;
    private readonly int maxNumberOfSentences = 4;
    private List<string> currentSentences;
    private List<int> choosedSentences;
    private NPC_Sentences_Recoverer recoverer;


    public List<string> CurrentSentences { get => currentSentences; set => currentSentences = value; }
    public int NumberOfSentences { get => numberOfSentences; set => numberOfSentences = value; }

    /// <summary>
    /// Function that is called right after the scene is loaded, initialize the variables
    /// </summary>
    private void Awake()
    {
        recoverer = GameObject.Find("Npc_Sentences").GetComponent<NPC_Sentences_Recoverer>();
        choosedSentences = new List<int>();
        currentSentences = new List<string>();
    }

    /// <summary>
    /// Select npc phrase/s, the number of phrases selected changes depending on the lenght of the phrases or according to their function
    /// </summary>
    public void selectNewSentences()
    {
        choosedSentences.Clear();
        currentSentences.Clear();
        List<string> allSentences;

        if (npcState == null || npcState.Length == 0)
        {
            allSentences = recoverer.RecoverFunctionSentences(npcFunction);
        }
        else
        {
            allSentences = recoverer.RecoverFunctionSentences(npcFunction + npcState);
        }

        if (allSentences != null)
        {
            for (int iteration = 0; iteration < NumberOfSentences; iteration++)
            {

                int selectedIndexSentence = selectRandom(allSentences.Count);

                if (selectedIndexSentence != -1)
                {
                    string selectedSentence = allSentences[selectedIndexSentence];
                    if (selectedSentence.Length > 134)
                    {
                        if (currentSentences.Count >= 2)
                        {
                            selectRandom(allSentences.Count);
                        }

                        iteration++;
                        int letterCounter = 125;
                        bool correct = true;
                        bool lastLine = false;

                        string[] splittedText = selectedSentence.Split(' ');

                        int splitNumer = splittedText.Length;

                        string finalPhrase = "";
                        for (int i = 0; i < splitNumer; i++)
                        {
                            correct = true;
                            finalPhrase += splittedText[i];

                            if (splitNumer - i != 1)
                            {
                                finalPhrase += " ";
                            }
                            else
                            {
                                lastLine = true;
                            }
                            if (!lastLine)
                            {
                                if (finalPhrase.Length + splittedText[i + 1].Length <= letterCounter)
                                {
                                    correct = false;
                                }
                            }

                            if (correct)
                            {
                                currentSentences.Add(finalPhrase);
                                finalPhrase = "";
                                correct = false;
                            }
                        }
                    }
                    else if (selectedSentence.Length < 67)
                    {
                        currentSentences.Add(selectedSentence);
                        if (allSentences.Count < choosedSentences.Count && choosedSentences.Count < maxNumberOfSentences)
                        {
                            iteration--;
                        }
                    }
                    else
                    {
                        currentSentences.Add(selectedSentence);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Select a random number between the given range
    /// </summary>
    /// <param name="sentecesCount">Number of phrases to be selected</param>
    /// <returns>Int, index of the selected phrase</returns>
    private int selectRandom(int sentecesCount)
    {
        int selectedIndexSentence = UnityEngine.Random.Range(0, sentecesCount);

        if (choosedSentences.Contains(selectedIndexSentence))
        {
            do
            {
                selectedIndexSentence = UnityEngine.Random.Range(0, sentecesCount);
            } while (choosedSentences.Contains(selectedIndexSentence));
        }
        else
        {
            choosedSentences.Add(selectedIndexSentence);
        }

        return selectedIndexSentence;
    }
}
