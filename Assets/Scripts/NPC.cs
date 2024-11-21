using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class NPC : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera dialogueCam;
    [SerializeField] TextAsset dialogue;
    private PlayerController player;

    bool dialogueExhausted = false;

    private void Start() 
    {
        //dialogue = GetComponent<DialogueManager>();    
    }

    private void OnTriggerEnter(Collider other) 
    {
        player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.SetInteractable(dialogueCam);
            player.NPCDialogue = dialogue;
            player.ActiveNPC = this;
        }
    }
    
    private void OnTriggerExit(Collider other) 
    {
        if (player != null)
        {
            player.ResetInteractable();
            player.NPCDialogue = null;
            player.ActiveNPC = null;
            player = null;
        }    
    }
}
