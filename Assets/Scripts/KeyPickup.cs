using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        StealthPlayerController dpc = collider.GetComponent<StealthPlayerController>();
        if (dpc != null)
        {
            dpc.hasKey = true;
            gameObject.SetActive(false);
        }
    }
}
