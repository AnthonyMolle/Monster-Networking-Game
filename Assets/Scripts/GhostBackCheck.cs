using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostBackCheck : MonoBehaviour
{
    [SerializeField] GhostEnemy ghostParent;

    private void OnTriggerEnter(Collider collider)
    {
        StealthPlayerController spc = collider.gameObject.GetComponent<StealthPlayerController>();

        if (spc != null)
        {
            spc.AddInRangeEnemy(ghostParent);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        StealthPlayerController spc = collider.gameObject.GetComponent<StealthPlayerController>();

        if (spc != null)
        {
            spc.RemoveInRangeEnemy(ghostParent);
        }
    }
}
