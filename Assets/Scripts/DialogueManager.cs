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

    [SerializeField] float timeBetweenCharacters = 0.1f;
    

    private Story currentStory;
    private NPC currentNPC;

    private void Start()
    {
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
    }

    public void StartDialogue()
    {
        StartCoroutine(DialogueLoop());
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

        awaitingPlayerContinue = true;
        dialoguePlaying = false;
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
