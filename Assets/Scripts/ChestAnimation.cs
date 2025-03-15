using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimation : MonoBehaviour
{
    [SerializeField] Chest chest;

    public void Win()
    {
        chest.AnimationWinTrigger();
    }
}
