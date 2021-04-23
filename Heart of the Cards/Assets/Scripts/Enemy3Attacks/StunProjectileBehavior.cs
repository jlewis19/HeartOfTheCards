using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunProjectileBehavior : MonoBehaviour
{

    public float speed = 75f;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float step = Time.deltaTime * speed;
        transform.position += step * transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.playerHealth.TakeDamage(PresidentAttacks.stunDamage);
            player.GetComponent<PlayerController>().isStunned = true;
        }
    }
}
