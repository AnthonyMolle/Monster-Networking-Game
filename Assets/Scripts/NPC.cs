using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NPC : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera dialogueCam;
    [SerializeField] DialogueManager dialogue;
    private PlayerController player;

    private void Start() 
    {
        dialogue = GetComponent<DialogueManager>();    
    }

    private void OnTriggerEnter(Collider other) 
    {
        player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.SetInteractable(dialogueCam);
        }
    }
    
    private void OnTriggerExit(Collider other) 
    {
        if (player != null)
        {
            player.ResetInteractable();
            player = null;
        }    
    }
}
