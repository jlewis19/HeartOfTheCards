using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttackBehavior : MonoBehaviour
{
    public float length = 17f;
    float beamTickTimer = 0f;
    bool doDamage = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!doDamage)
        {
            beamTickTimer += Time.deltaTime;
            if (beamTickTimer >= EnemyAttacks.beamTick)
            {
                beamTickTimer = 0f;
                doDamage = true;
            }
        }
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, length, transform.localScale.z), Time.deltaTime * EnemyAttacks.beamSpeed);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && doDamage)
        {
            LevelManager.playerHealth.TakeDamage(EnemyAttacks.BeamDamage);
            doDamage = false;
        }
    }
}
