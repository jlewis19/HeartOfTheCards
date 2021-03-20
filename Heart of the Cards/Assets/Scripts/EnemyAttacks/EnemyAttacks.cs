using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [Header("Boss Attack Damage Values")]
    public static int AntiCampingProjectileDamage = 10;
    public static int BeamDamage = 2;
    public static int WaveDamage = 20;

    [Header("Projectile Prefabs")]
    public GameObject antiCampingProjectile;
    public GameObject beamProjectile;
    public GameObject waveProjectile;

    [Header("Beam Attack Related")]
    public Transform centerOfRoom;
    public static float beamSpeed = 2f;
    public static float beamTick = .3f;

    bool antiCampingSpawned = false;
    bool beamSpawned = false;
    bool waveSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //AntiCampingAttack();
        //BeamAttack();
        WaveAttack();
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

    void BeamAttack()
    {
        float step = 5 * Time.deltaTime;
        if (Vector3.Distance(transform.position, centerOfRoom.position) > 0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position, centerOfRoom.position, step);
        }
        else
        {
            // Start Attack
            if (!beamSpawned)
            {
                beamSpawned = true;
                Instantiate(beamProjectile, transform.position + Vector3.down, 
                    Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z + 90));
                Instantiate(beamProjectile, transform.position + Vector3.down,
    Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 45, transform.rotation.eulerAngles.z + 90));
                Instantiate(beamProjectile, transform.position + Vector3.down,
    Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 135, transform.rotation.eulerAngles.z + 90));
                Instantiate(beamProjectile, transform.position + Vector3.down,
                    Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 90));
            }
        }
    }

    void WaveAttack()
    {
        if (!waveSpawned)
        {
            waveSpawned = true;
            Instantiate(waveProjectile, transform.position, transform.rotation);
        }
    } 
}
