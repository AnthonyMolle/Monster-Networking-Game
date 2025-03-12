using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    DungeonPlayerController storedPlayer;
    [SerializeField] Animator chestAnimator;
    [SerializeField] GameObject interactionUI;

    void OnTriggerEnter(Collider collider)
    {
        DungeonPlayerController dpc = collider.GetComponent<DungeonPlayerController>();
        if (dpc != null)
        {
            interactionUI.SetActive(true);
            storedPlayer = dpc;
            dpc.SetInteractable(this);
            dpc.chest = this;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        DungeonPlayerController dpc = collider.GetComponent<DungeonPlayerController>();
        if (dpc != null)
        {
            interactionUI.SetActive(false);
            storedPlayer = null;
            dpc.ResetInteractable();
            dpc.chest = null;
        }
    }


    public void WinAnimation()
    {
        if (storedPlayer != null)
        {
            chestAnimator.Play("ChestOpen");
            interactionUI.SetActive(false);
        }
    }

    public void AnimationWinTrigger()
    {
        if (storedPlayer != null)
        {
            storedPlayer.Win();
        }
    }
}
