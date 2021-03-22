using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBehavior : MonoBehaviour
{
    public GameObject explosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Instantiate(explosion, transform.position, transform.rotation);
            LevelManager.playerHealth.TakeDamage(EnemyAttacks.MineDamage);
            gameObject.SetActive(false);
            Destroy(gameObject, 0.5f);
        }
    }
}
