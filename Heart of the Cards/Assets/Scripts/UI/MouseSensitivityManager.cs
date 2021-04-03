using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityManager : MonoBehaviour
{
    public Slider sensitivitySlider;
    public GameObject sensitivityMenu;

    bool active;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            active = !active;
            sensitivityMenu.SetActive(active);
        }
    }

    public void UpdateSensitivity()
    {
        LevelManager.mouseSensitivity = sensitivitySlider.value;
    }
}
