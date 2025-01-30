using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScreen : MonoBehaviour
{
    public CombatManager cm;

    public void SwapCharacters()
    {
        cm.SwapPlayer();
    }

    public void DeactivateSelf()
    {
        gameObject.SetActive(false);
    }
}
