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
    GameObject mainPlayer;
    //GameObject preCombatCam;

    public void StartCombat(GameObject player)
    {
        transitionScreen.gameObject.SetActive(true);
        transitionScreen.cm = this;
        mainPlayer = player;
    }

    public void SwapPlayer()
    {
        if (dungeonPlayer.activeSelf)
        {
            dungeonPlayer.SetActive(false);
            mainPlayer.SetActive(true);
        }
        else
        {
            dungeonPlayer.SetActive(true);
            mainPlayer.SetActive(false);
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
