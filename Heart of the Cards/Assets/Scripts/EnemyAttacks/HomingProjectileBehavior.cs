using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectileBehavior : MonoBehaviour
{
    public GameObject player;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cover"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            LevelManager.playerHealth.TakeDamage(EnemyAttacks.HomingDamage);
            Destroy(gameObject);
        }
    }
}
