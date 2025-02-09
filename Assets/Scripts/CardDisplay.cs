using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    CombatManager cm;

    [SerializeField] MeshRenderer cardMeshRenderer;
    [SerializeField] MeshFilter cardMeshFilter;
    [SerializeField] Animator cardDisplayAnimator;

    [SerializeField] Button button;

    public void InitializeCardDisplay(CombatManager sendingCM, Mesh cardMesh, Material cardMat)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button.gameObject);

        cm = sendingCM;

        cardMeshRenderer.material = cardMat;
        cardMeshFilter.mesh = cardMesh;
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
