using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBehavior : MonoBehaviour
{
    public float atkDuration = 15;
    public float rotationSpeed = 5;
    public Transform rotationAxis;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        rotationAxis = enemy.transform;
        Destroy(gameObject, atkDuration);
    }

    // Update is called once per frame
    void Update()
    {
        rotationAxis = enemy.transform;
        transform.RotateAround(rotationAxis.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            LevelManager.playerHealth.TakeDamage(WarAttacks.RotatingProj);
        }
    }
}
