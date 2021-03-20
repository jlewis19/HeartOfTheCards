using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAttackBehavior : MonoBehaviour
{
    public float growthSpeed = .1f;
    public float atkDuration = 5;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, atkDuration);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newSize = transform.localScale;
        newSize.x += growthSpeed;
        newSize.z += growthSpeed;
        transform.localScale = newSize;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.playerHealth.TakeDamage(EnemyAttacks.WaveDamage);
        }
    }
}
