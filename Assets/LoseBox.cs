using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoseBox : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        DungeonPlayerController dpc = collider.GetComponent<DungeonPlayerController>();
        if (dpc != null)
        {
            dpc.TakeDamage(100000);
        }
    }
}
