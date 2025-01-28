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
    [SerializeField] GameObject nameBox;
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

    public void InitializeDialogue(TextAsset inkDialogue, string npcName = null, NPC sendingNPC = null)
    {
        dialogueBox.SetActive(true);
        currentStory = new Story(inkDialogue.text);
        if (npcName != null)
        {
            nameText.text = npcName;
            nameBox.SetActive(true);
        }
        else
        {
            nameBox.SetActive(false);
        }

        if (sendingNPC != null)
        {
            currentNPC = sendingNPC;
            currentStory.BindExternalFunction("StartCombat", () => 
            {
                currentNPC.StartCombat();
            });

            currentStory.BindExternalFunction("ExhaustDialogue", () => 
            {
                currentNPC.exhausted = true;
            });

            currentStory.BindExternalFunction("ShowName", (string name) => 
            {
                nameText.text = name;
                currentNPC.showName = true;
            });
        }
        else
        {
            currentNPC = null;
        }

        currentStory.BindExternalFunction("ReactivatePlayer", (bool delay) => 
        {
            if (delay)
            {
                pc.ReactivatePlayerDelay();
            }
            else
            {
                pc.ReactivatePlayer();
            }
        });
    }

    // start dialogue is called in an animation event on the dialogue box at the end of its spawn in animation
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
    }

    public void DeactivateDialogueBox()
    {
        dialogueBox.SetActive(false);
    }
}
