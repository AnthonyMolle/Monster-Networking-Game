using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    float storedTimeScale;
    GameObject storedSelectedButton;
    [SerializeField] GameObject firstSelected;
    [SerializeField] GameObject optionsMenu;

    void OnEnable()
    {
        storedTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        storedSelectedButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void Unpause()
    {
        gameObject.SetActive(false);
    }

    public void GoToMainMenu()
    {

    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
    }

    void OnDisable()
    {
        Time.timeScale = storedTimeScale;
        EventSystem.current.SetSelectedGameObject(storedSelectedButton);
    }
}
