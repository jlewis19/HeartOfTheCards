using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public Slider healthBar;

    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        healthBar.maxValue = startingHealth;
        SetHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }

    public void takeDamage(int amount) {
        currentHealth -= amount;
        SetHealthBar();
        Debug.Log("Current enemy health: " + currentHealth);
    }

    public void HealUp(int amount) 
    {
        if (currentHealth / startingHealth > .5f) 
        {
            currentHealth += amount;
            SetHealthBar();
            Debug.Log("Enemy recovered and now has " + currentHealth);
        }
    }

    private void SetHealthBar()
    {
        healthBar.value = currentHealth;
    }
}
