using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class BombAnimationEvents : MonoBehaviour
{
    public void ResetBombThrow()
    {
        FindObjectOfType<BombPlayer>().ResetThrow();
    }
}
