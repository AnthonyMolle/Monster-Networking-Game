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

    [SerializeField] string name;
    [SerializeField] string mysteryName = "???";

    [SerializeField] CombatManager personalCM;

    [SerializeField] public Mesh cardMesh;
    [SerializeField] public Material cardMaterial;

    public bool exhausted = false;
    public bool tryAgain = false;
    public bool won = false;
    public bool showName = false;

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
        if (showName)
        {
            if (exhausted)
            {
                dm.InitializeDialogue(exhaustedDialogue, name, this);
            }
            else if (won)
            {
                dm.InitializeDialogue(winDialogue, name, this);
            }
            else if (tryAgain)
            {
                dm.InitializeDialogue(tryAgainDialogue, name, this);
            }
            else
            {
                dm.InitializeDialogue(mainDialogue, name, this);
            }
        }
        else
        {
            if (exhausted)
            {
                dm.InitializeDialogue(exhaustedDialogue, mysteryName, this);
            }
            else if (won)
            {
                dm.InitializeDialogue(winDialogue, mysteryName, this);
            }
            else if (tryAgain)
            {
                dm.InitializeDialogue(tryAgainDialogue, mysteryName, this);
            }
            else
            {
                dm.InitializeDialogue(mainDialogue, mysteryName, this);
            }
        }
    }

    public void StartCombat()
    {
        personalCM.StartCombat(player.transform.parent.gameObject);
    }
}
