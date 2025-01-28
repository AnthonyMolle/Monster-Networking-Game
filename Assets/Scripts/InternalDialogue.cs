using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InternalDialogue : MonoBehaviour
{
    [SerializeField] TextAsset dialogue;
    BoxCollider boxCollider;
    MeshRenderer mr;
    DialogueManager dm;
    PlayerController player;

    private void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
        boxCollider = GetComponent<BoxCollider>();

        // test
        mr = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        player = collider.GetComponent<PlayerController>();
        if (player != null)
        {
            StartInternalDialogue();
            player.DeactivatePlayer();
        }
    }

    private void StartInternalDialogue()
    {
        dm.InitializeDialogue(dialogue);
        boxCollider.enabled = false;
        mr.enabled = false;
    }
}
