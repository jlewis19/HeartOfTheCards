using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBoss : EnemyHealth
{

    Vector3 deathPosition;

    // Update is called once per frame
    protected override void Update()
    {
        if (currentHealth <= 0)
        {
            deathPosition = transform.position;
            anim.SetInteger("animState", 1);
            Invoke("enableObject", 5);
            Invoke("disableObject", 3);
        }
    }

    void disableObject()
    {
        gameObject.SetActive(false);
        anim.SetInteger("animState", 0);
    }

    void enableObject()
    {
        gameObject.transform.position = deathPosition;
        gameObject.SetActive(true);
        currentHealth = startingHealth;
        healthBar.value = startingHealth;
    }
}
