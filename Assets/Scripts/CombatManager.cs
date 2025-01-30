using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] NPC attachedNPC;
    [SerializeField] GameObject cardDisplayObject;
    [SerializeField] TransitionScreen transitionScreen;
    //[SerializeField] GameObject combatCam;
    [SerializeField] GameObject dungeonPlayer;
    //GameObject preCombatCam;

    public void StartCombat()
    {
        transitionScreen.gameObject.SetActive(true);
        transitionScreen.cm = this;
    }

    public void SwapPlayer()
    {
        if (dungeonPlayer.activeSelf)
        {
            dungeonPlayer.SetActive(false);
        }
        else
        {
            dungeonPlayer.SetActive(true);
        }
    }

    private void Update()
    {

    }

    public void Win()
    {
        attachedNPC.won = true;
        cardDisplayObject.SetActive(true);
        cardDisplayObject.GetComponent<CardDisplay>().InitializeCardDisplay(this, attachedNPC.cardMesh, attachedNPC.cardMaterial);
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

        transitionScreen.gameObject.SetActive(true);
        //FindObjectOfType<PlayerController>().ReactivatePlayer();
    }
}
