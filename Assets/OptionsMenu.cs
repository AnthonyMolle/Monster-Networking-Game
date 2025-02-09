using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    GameObject storedSelectedButton;
    [SerializeField] GameObject firstSelected;

    void OnEnable()
    {
        storedSelectedButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void ExitOptions()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(storedSelectedButton);
    }
}
