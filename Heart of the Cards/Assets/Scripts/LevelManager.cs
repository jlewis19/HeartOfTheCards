using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int projectileDamage = 10;
    public static GameObject player;
    public static PlayerHealth playerHealth;
    public Text gameOverText;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        gameOverText.enabled = false;
    }
    /*

    // Update is called once per frame
    void Update()
    {
        
    }*/
    public void PlayerDies()
    {
        gameOverText.enabled = true;
        Invoke("LoadCurrentLevel", 2);
    }

    public void EnemyDies() {
        gameOverText.text = "You win!!!";
        gameOverText.enabled = true;
    }

    void LoadCurrentLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
