using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MouseSensitivityManager : MonoBehaviour
{
    public Slider sensitivitySlider;
    public GameObject menu;
    public TextMeshProUGUI sensitivtyText;
    public GameObject cards;

    public static bool active;

    private void Start()
    {
        sensitivitySlider.value = LevelManager.mouseSensitivity;
        sensitivtyText.text = sensitivitySlider.value.ToString("f0");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (active)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        active = true;
        Time.timeScale = 0f;
        menu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        cards.SetActive(false);
    }

    public void ResumeGame()
    {
        active = false;
        Time.timeScale = 1f;
        menu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cards.SetActive(true);
    }

    public void UpdateSensitivity()
    {
        LevelManager.mouseSensitivity = sensitivitySlider.value;
        sensitivtyText.text =sensitivitySlider.value.ToString("f0");
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        active = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu() {
        Time.timeScale = 1f;
        active = false;
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        active = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
