using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralProj : MonoBehaviour
{
    public float speed = 2f;

    GameObject player;
    GameObject boss;

    void Start()
    {
        Destroy(gameObject, 5);
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        float step = Time.deltaTime * speed;
        Vector3 target = new Vector3(boss.transform.position.x, transform.position.y, boss.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, -1 * step);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            LevelManager.playerHealth.TakeDamage(PresidentAttacks.spiralDamage);
        }
    }
}
