using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerAnimations : MonoBehaviour
{
    [SerializeField] StealthPlayerController spc;

    public void ResetDagger()
    {
        spc.daggerRecovering = false;
    }
}
