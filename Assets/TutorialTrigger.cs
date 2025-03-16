using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] GameObject tutorialUI;

    void OnTriggerEnter(Collider collider)
    {
        DungeonPlayerController dpc = collider.GetComponent<DungeonPlayerController>();
        if (dpc != null)
        {
            tutorialUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        DungeonPlayerController dpc = collider.GetComponent<DungeonPlayerController>();
        if (dpc != null)
        {
            tutorialUI.SetActive(false);
        }
    }
}
