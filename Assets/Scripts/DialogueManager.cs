using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject dialogueBox;
    [SerializeField] Animator dialogueBoxAnimator;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;

    [SerializeField] GameObject[] choiceButtons;
    TextMeshProUGUI[] choiceText;

    [SerializeField] PlayerController pc;

    [SerializeField] float timeBetweenCharacters = 0.1f;
    

    private Story currentStory;
    private NPC currentNPC;

    private void Start()
    {
        choiceText = new TextMeshProUGUI[choiceButtons.Length];

        int i = 0;
        foreach(GameObject choiceButton in choiceButtons)
        {
            choiceText[i] = choiceButton.GetComponentInChildren<TextMeshProUGUI>();
            choiceButton.SetActive(false);
            i++;
        }

        dialogueBox.SetActive(false);
        dialogueText.text = "";
        nameText.text = "";
    }

    bool skipDialogue = false;

    private void Update()
    {
        if (awaitingPlayerContinue)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartDialogue();
            }
        }
        else if (dialoguePlaying)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                skipDialogue = true;
            }
        }
    }

    public void InitializeDialogue(TextAsset inkDialogue, NPC sendingNPC)
    {
        dialogueBox.SetActive(true);
        currentStory = new Story(inkDialogue.text);
        currentNPC = sendingNPC;

        currentStory.BindExternalFunction("NextDialogue", () => 
        {
            
        });
    }

    public void StartDialogue()
    {
        if (currentStory.canContinue)
        {
            StartCoroutine(DialogueLoop());
        }
        else
        {
            EndDialogue();
        }
    }

    bool awaitingPlayerContinue = false;
    bool dialoguePlaying = false;

    private IEnumerator DialogueLoop()
    {
        dialogueText.text = currentStory.Continue();
        dialogueText.maxVisibleCharacters = 0;
        awaitingPlayerContinue = false;
        dialoguePlaying = true;

        foreach (char character in dialogueText.text.ToCharArray())
        {
            if (skipDialogue)
            {
                dialogueText.maxVisibleCharacters = dialogueText.text.ToCharArray().Length;
                skipDialogue = false;
                break;
            }

            dialogueText.maxVisibleCharacters += 1;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }

        if (currentStory.currentChoices.Count > 0)
        {
            dialoguePlaying = false;
            DisplayChoices();
        }
        else if (!currentStory.canContinue)
        {
            EndDialogue();
        }
        else
        {
            awaitingPlayerContinue = true;
            dialoguePlaying = false;
        }
    }

    private void DisplayChoices()
    {
        for (int i = 0; i < currentStory.currentChoices.Count; i += 1)
        {
            choiceButtons[i].SetActive(true);
            choiceText[i].text = currentStory.currentChoices[i].text;
        }
    }

    private void HideChoices()
    {
        int i = 0;
        foreach(GameObject choiceButton in choiceButtons)
        {
            choiceButton.SetActive(false);
            i++;
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        HideChoices();
        StartDialogue();
    }

    public void EndDialogue()
    {
        dialogueBoxAnimator.Play("DialogueBoxExit");
        pc.ReactivatePlayer();
    }

    public void DeactivateDialogueBox()
    {
        dialogueBox.SetActive(false);
    }
}
