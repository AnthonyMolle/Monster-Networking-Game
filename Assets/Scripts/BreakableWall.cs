using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public Rock[] rocks;
    [SerializeField] float rockShrinkDelay = 3f;

    public void ActivateRocks()
    {
        foreach (Rock rock in rocks)
        {
            rock.ActivateRock(rockShrinkDelay);
        }

        gameObject.GetComponent<Collider>().enabled = false;
    }

}
