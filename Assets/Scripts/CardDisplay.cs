using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    CombatManager cm;

    [SerializeField] MeshRenderer cardMeshRenderer;
    [SerializeField] MeshFilter cardMeshFilter;
    [SerializeField] Animator cardDisplayAnimator;

    public void InitializeCardDisplay(CombatManager sendingCM, Mesh cardMesh = null, Material cardMat = null)
    {
        cm = sendingCM;

        //cardMeshRenderer.material = cardMat;
        //cardMeshFilter.mesh = cardMesh;
    }

    public void StartClose()
    {
        cardDisplayAnimator.Play("CardScreenDisappear");
    }

    public void CloseCardDisplay()
    {
        cm.EndCombat();
        gameObject.SetActive(false);
    }

}
