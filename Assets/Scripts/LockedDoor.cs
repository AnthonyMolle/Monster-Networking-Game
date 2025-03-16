using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    StealthPlayerController storedPlayer;
    [SerializeField] Animator doorAnimator;
    [SerializeField] GameObject interactionUI;
    // [SerializeField] GameObject lockObject;
    [SerializeField] BoxCollider boundsChecker;

    public void OpenDoor()
    {
        // lockObject.SetActive(false);
        interactionUI.SetActive(false);
        if (storedPlayer != null)
        {
            storedPlayer.ResetInteractable();
            storedPlayer.activeDoor = null;
            storedPlayer = null;
        }
        boundsChecker.enabled = false;
        doorAnimator.Play("OpenDoor");
    }

    void OnTriggerEnter(Collider collider)
    {
        StealthPlayerController dpc = collider.GetComponent<StealthPlayerController>();
        if (dpc != null)
        {
            if (dpc.hasKey)
            {
                interactionUI.SetActive(true);
                storedPlayer = dpc;
                dpc.SetInteractable();
                dpc.activeDoor = this;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        StealthPlayerController dpc = collider.GetComponent<StealthPlayerController>();
        if (dpc != null)
        {
            interactionUI.SetActive(false);
            storedPlayer = null;
            dpc.ResetInteractable();
            dpc.activeDoor = null;
        }
    }

    public void Reset()
    {
        doorAnimator.Play("DoorClosed");
        storedPlayer = null;
        boundsChecker.enabled = true;
    }
}
