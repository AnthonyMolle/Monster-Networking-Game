using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] NPC attachedNPC;
    [SerializeField] GameObject cardDisplayObject;
    [SerializeField] GameObject combatCam;
    GameObject preCombatCam;

    public void StartCombat()
    {
        preCombatCam = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject;
        preCombatCam.SetActive(false);
        combatCam.SetActive(true);
        //FindObjectOfType<PlayerController>().DeactivatePlayer();
    }

    private void Update()
    {
        // TESTING PURPOSES
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Win();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Lose();
        }
        // TESTING PURPOSES
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

        combatCam.SetActive(false);
        preCombatCam.SetActive(true);
        preCombatCam = null;
        //FindObjectOfType<PlayerController>().ReactivatePlayer();
    }
}
