using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MouseSensitivityManager : MonoBehaviour
{
    public Slider sensitivitySlider;
    public GameObject sensitivityMenu;
    public TextMeshProUGUI sensitivtyText;

    public static bool active;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (active)
            {
                active = false;
                Time.timeScale = 1f;
                sensitivityMenu.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                active = true;
                Time.timeScale = 0f;
                sensitivityMenu.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void UpdateSensitivity()
    {
        LevelManager.mouseSensitivity = sensitivitySlider.value;
        sensitivtyText.text =sensitivitySlider.value.ToString("f0");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
