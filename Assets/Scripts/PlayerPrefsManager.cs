using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefsManager : MonoBehaviour
{
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] Slider volumeSlider;

    [SerializeField] PlayerController pc;
    [SerializeField] DungeonPlayerController[] dpcs;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("Sensitivity", -1) == -1)
        {
            PlayerPrefs.SetInt("Sensitivity", (int)sensitivitySlider.value);
        }
        else
        {
            sensitivitySlider.value = PlayerPrefs.GetInt("Sensitivity");
        }
    }

    public void UpdateSensitivity()
    {
        PlayerPrefs.SetInt("Sensitivity", (int)sensitivitySlider.value);

        pc.UpdateSensitivity();

        foreach (DungeonPlayerController dpc in dpcs)
        {
            dpc.UpdateSensitivity();
        }
    }

    public void UpdateVolume(int Volume)
    {

    }
}
