using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int projectileDamage = 10;
    public static float mouseSensitivity = 100;
    public static GameObject player;
    public static PlayerHealth playerHealth;
    public Text gameOverText;

    public AudioClip winSFX;
    public AudioClip loseSFX;

    bool gameOver = false;
    
    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        gameOverText.enabled = false;
    }

    public void PlayerDies()
    {
        if (!gameOver) {
            gameOver = true;
            AudioSource.PlayClipAtPoint(loseSFX, player.transform.position);
            gameOverText.text = "You lose :(";
            gameOverText.enabled = true;
            Invoke("LoadCurrentLevel", 2);
        }
    }

    public void EnemyDies() {
        if (!gameOver) {
            gameOver = true;
            AudioSource.PlayClipAtPoint(winSFX, player.transform.position);
            gameOverText.text = "You win!!!";
            gameOverText.enabled = true;
            Invoke("LoadNextLevel", 2);
        }
    }

    void LoadCurrentLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void LoadNextLevel() {
        int level = SceneManager.GetActiveScene().buildIndex + 1;
        PlayerPrefs.SetInt("currentLevel", level);
        SceneManager.LoadScene(level);
    }
}
