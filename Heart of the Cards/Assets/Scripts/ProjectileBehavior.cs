using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public int damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Minion")) {
            var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.takeDamage(damage);
        }
    }
}
