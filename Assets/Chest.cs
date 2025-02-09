using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        DungeonPlayerController dpc = collider.GetComponent<DungeonPlayerController>();
        if (dpc != null)
        {
            dpc.SetInteractable(this);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        DungeonPlayerController dpc = collider.GetComponent<DungeonPlayerController>();
        if (dpc != null)
        {
            dpc.ResetInteractable();
        }
    }
}
