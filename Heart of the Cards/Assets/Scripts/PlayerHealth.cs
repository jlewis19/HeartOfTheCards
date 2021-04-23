using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    public int startingHealth = 100;
    public Slider healthSlider;
    public AudioClip hitSFX;
    public int reductionAmount = 2;
    public bool hasArmor = false;

    int currentHealth;
    bool isDead = false;

    // Start is called before the first frame update
    void Start() {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damageAmount) {
        AudioSource.PlayClipAtPoint(hitSFX, transform.position);

        if (hasArmor) {
            damageAmount /= reductionAmount;
            hasArmor = false;
        }

        if (currentHealth > 0 && !PlayerController.dashing) {
            currentHealth -= damageAmount;
            healthSlider.value = currentHealth;
            Mathf.Clamp(currentHealth, 0, 100);
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

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            TakeDamage(10);
        }
    }
}
