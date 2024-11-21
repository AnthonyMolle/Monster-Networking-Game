using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    //these are functions activated through animation events, and essentially tell the dialoguemanager when they should start writing dialogue/should deactivate the dialogue box
    public void InitiateDialogueWriting()
    {
        dialogueManager.StartDialogue();
    }

    public void DisableDialogueBox()
    {
        dialogueManager.DeactivateDialogueBox();
    }
}
