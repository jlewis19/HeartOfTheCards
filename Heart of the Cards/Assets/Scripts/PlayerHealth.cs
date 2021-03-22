using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    public int startingHealth = 100;
    public Slider healthSlider;

    int currentHealth;
    bool isDead = false;

    // Start is called before the first frame update
    void Start() {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update() {

    }

    public void TakeDamage(int damageAmount) {
        if (currentHealth > 0) {
            currentHealth -= damageAmount;
            healthSlider.value = currentHealth;
            
        }
        if (currentHealth <= 0 && !isDead) {
            isDead = true; 
            PlayerDies();
        }

        Debug.Log("Current health: " + currentHealth);
    }

    void PlayerDies() {
        Debug.Log("Player is dead");
        transform.Rotate(-90, 0, 0, Space.Self);
        FindObjectOfType<LevelManager>().PlayerDies();
    }
}
