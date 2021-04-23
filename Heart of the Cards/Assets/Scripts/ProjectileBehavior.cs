﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public int damage = 10;
    
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Minion")) {
            var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.takeDamage(damage);
        }
    }
}
