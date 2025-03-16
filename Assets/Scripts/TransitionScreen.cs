using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScreen : MonoBehaviour
{
    public CombatManager cm;
    [SerializeField] GameObject interactionUI;

    public void SwapCharacters()
    {
        cm.SwapPlayer();
    }

    public void DeactivateSelf()
    {
        gameObject.SetActive(false);
    }

    public void StartSendingDialogue()
    {
        cm.SendPostCombatDialogue();
    }

    public void DeactivateInteractUI()
    {
        if (interactionUI.activeSelf == true)
        {
            interactionUI.SetActive(false);
        }
    }

    public NPC npc;

    public void FireNPCDialogue()
    {
        if (npc != null)
        {
            npc.SendDialogue();
            npc = null;
        }
    }
}
