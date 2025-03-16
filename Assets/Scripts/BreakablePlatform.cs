using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
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

    public void Reset()
    {
        gameObject.GetComponent<Collider>().enabled = true;

        foreach (Rock rock in rocks)
        {
            if (rock.gameObject.activeSelf)
            {
                rock.Reset();
                rock.gameObject.SetActive(true);
            }
            else
            {
                rock.gameObject.SetActive(true);
            }
        }
    }

}
