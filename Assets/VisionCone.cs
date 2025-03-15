using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        DungeonPlayerController dpc = collider.GetComponent<DungeonPlayerController>();
        if (dpc != null)
        {
            dpc.TakeDamage(10000);
        }
    }
}
