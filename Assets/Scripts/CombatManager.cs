using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] NPC attachedNPC;
    [SerializeField] GameObject cardDisplayObject;

    //TEMP FOR TESTING
    [SerializeField] GameObject uiCombat;

    public void StartCombat()
    {
        //TEMP FOR TESTING
        uiCombat.SetActive(true);
        //FindObjectOfType<PlayerController>().DeactivatePlayer();
    }

    public void Win()
    {
        attachedNPC.won = true;
        cardDisplayObject.SetActive(true);
        cardDisplayObject.GetComponent<CardDisplay>().InitializeCardDisplay(this);
    }

    public void Lose()
    {
        EndCombat();
        attachedNPC.tryAgain = true;
        attachedNPC.SendDialogue();
    }

    public void EndCombat()
    {
        // exit combat somehow
        if (attachedNPC.won == true)
        {
            attachedNPC.SendDialogue();
        }

        //TEMP TEST
        uiCombat.SetActive(false);
        //FindObjectOfType<PlayerController>().ReactivatePlayer();
    }
}
