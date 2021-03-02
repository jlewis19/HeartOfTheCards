using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;

    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth < 0) {
            Destroy(gameObject);
        }
    }

    public void takeDamage(int amount) {
        currentHealth -= amount;
        Debug.Log("Current enemy health: " + currentHealth);
    }
}
