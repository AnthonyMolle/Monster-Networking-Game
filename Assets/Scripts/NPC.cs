using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class NPC : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera dialogueCam;
    [SerializeField] public TextAsset mainDialogue;
    [SerializeField] public TextAsset tryAgainDialogue;
    [SerializeField] public TextAsset winDialogue;
    [SerializeField] public TextAsset exhaustedDialogue;

    [SerializeField] CombatManager personalCM;

    public bool exhausted = false;
    public bool tryAgain = false;
    public bool won = false;

    private PlayerController player;
    private DialogueManager dm;

    private void Start() 
    {
        dm = FindObjectOfType<DialogueManager>();    
    }

    private void OnTriggerEnter(Collider other) 
    {
        player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.SetInteractable(dialogueCam);
            //player.NPCDialogue = dialogue;
            player.activeNPC = this;
        }
    }
    
    private void OnTriggerExit(Collider other) 
    {
        if (player != null)
        {
            player.ResetInteractable();
            //player.NPCDialogue = null;
            player.activeNPC = null;
            player = null;
        }    
    }

    public void SendDialogue()
    {
        if (exhausted)
        {
            dm.InitializeDialogue(exhaustedDialogue, this);
        }
        else if (won)
        {
            dm.InitializeDialogue(winDialogue, this);
        }
        else if (tryAgain)
        {
            dm.InitializeDialogue(tryAgainDialogue, this);
        }
        else
        {
            dm.InitializeDialogue(mainDialogue, this);
        }
        
    }

    public void StartCombat()
    {
        personalCM.StartCombat();
    }
}
