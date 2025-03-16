using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InternalDialogue : MonoBehaviour
{
    [SerializeField] TextAsset dialogue;
    BoxCollider boxCollider;
    DialogueManager dm;
    PlayerController player;
    DungeonPlayerController dpc;

    private void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        player = collider.GetComponent<PlayerController>();
        if (player != null)
        {
            StartInternalDialogue();
            player.DeactivatePlayer();
        }

        dpc = collider.GetComponent<DungeonPlayerController>();
        if (dpc != null)
        {
            StartInternalDialogue();
            dpc.DeactivatePlayer();
        }
    }

    private void StartInternalDialogue()
    {
        dm.InitializeDialogue(dialogue);
        boxCollider.enabled = false;
    }

    public void Activate()
    {
        boxCollider.enabled = true;
    }
}
