using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public Rigidbody[] rbs;

    public void ActivateRocks()
    {
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
        }

        gameObject.GetComponent<Collider>().enabled = false;
    }
}
