using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textObject;

    [SerializeField] string[] sentences;
    [SerializeField] string[] exhaustedSentences;
    [SerializeField] AudioSource[] voiceClips;

    [System.Serializable]
    public class ChoiceButtons
    {
        public GameObject[] buttons;
    }

    public ChoiceButtons[] choiceButtons;

    [SerializeField] float textSpeed;
    [SerializeField] float textPauseSpeed;
    [SerializeField] float startDelay;

    private int charIndex = 0;
    private int sentenceIndex = 0;
    private int buttonListIndex = 0;

    private bool textTyping = false;
    private bool typingInterrupted = false;
    private bool choosing = false;

    public void StartDialogue()
    {
        StartCoroutine(DialogueStartup());
    }

    private IEnumerator DialogueStartup()
    {
        //ui initialization stuff
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(DialogueLoop());
    }

    private IEnumerator DialogueLoop()
    {
        textTyping = true;
        charIndex = 0;
        textObject.text = "";
        while (charIndex < sentences[sentenceIndex].Length)
        {
            if (textTyping == false)
            {
                textObject.text = sentences[sentenceIndex];
                charIndex = sentences[sentenceIndex].Length;
                break;
            }

            if (sentences[sentenceIndex][charIndex] != ">"[0])
            {
                yield return new WaitForSeconds(textSpeed);
                textObject.text += sentences[sentenceIndex][charIndex];
                if (sentences[sentenceIndex][charIndex] == '.')
                {
                    yield return new WaitForSeconds(textPauseSpeed);
                }
            }
            charIndex += 1;
        }

        if (sentences[sentenceIndex][charIndex - 1] == '>')
        {
            textObject.text = textObject.text.Substring(0, textObject.text.Length - 1);
            PresentChoices();
            //FindObjectOfType<PlayerController>().ReactivatePlayer();
        }
        else
        {
            sentenceIndex += 1;
            textTyping = false;
        }
    }

    public bool NextSentence()
    {
        if (sentenceIndex < sentences.Length)
        {
            StartCoroutine(DialogueLoop());
            return true;
        }

        return false;
    }

    private void PresentChoices()
    {
        choosing = true;
        for (int buttonIndex = 0; buttonIndex < choiceButtons[buttonListIndex].buttons.Length; buttonIndex += 1)
        {
            choiceButtons[buttonListIndex].buttons[buttonIndex].SetActive(true);
        }
    }

    public void Choose(int sendToIndex)
    {
        for (int buttonIndex = 0; buttonIndex < choiceButtons[buttonListIndex].buttons.Length; buttonIndex += 1)
        {
            choiceButtons[buttonListIndex].buttons[buttonIndex].SetActive(false);
        }

        buttonListIndex += 1;

        sentenceIndex = sendToIndex;
        choosing = false;
        StartCoroutine(DialogueLoop());
    }

    public void SendInput()
    {
        if (!choosing)
        {
            if (textTyping)
            {
                textTyping = false;
            }
            else if (NextSentence() == false)
            {
                textObject.text = "";
                //reset ui elements
                FindObjectOfType<PlayerController>().ReactivatePlayer();
            }
        }
    }
}
