using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    DialogueManager dm;
    CombatManager cm;

    [SerializeField] MeshRenderer cardMeshRenderer;
    [SerializeField] MeshFilter cardMeshFilter;
    [SerializeField] Animator cardDisplayAnimator;

    private void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
    }

    public void InitializeCardDisplay(CombatManager sendingCM, Mesh cardMesh, Material cardMat)
    {
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
        // end combat scene, and the end combat scene function should handle interactions with the dialogue manager to start the new dialogue
        gameObject.SetActive(false);
    }

}
