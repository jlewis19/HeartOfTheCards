using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{

    //public GameObject[] antiCampingProjectileWaypoints;
    public GameObject antiCampingProjectile;

    bool antiCampingSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AntiCampingAttack();
    }

    void AntiCampingAttack()
    {
        if (!antiCampingSpawned)
        {
            antiCampingSpawned = true;
            for (int ii = 0; ii < 4; ++ii)
            {
                antiCampingProjectile.GetComponent<AntiCampingProjectileBehavior>().CreateProjectile(ii * 2);
            }
        }
    }
}
