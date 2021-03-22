using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [Header("Boss Attack Damage Values")]
    public static int AntiCampingProjectileDamage = 10;
    public static int BeamDamage = 2;
    public static int WaveDamage = 20;
    public static int HomingDamage = 5;
    public static int MineDamage = 30;
    public static int healAmount = 30;
    public static float buffAmount = 1.5f;

    [Header("Projectile Prefabs")]
    public GameObject antiCampingProjectile;
    public GameObject beamProjectile;
    public GameObject waveProjectile;
    public GameObject homingProjectile;
    public GameObject minePrefab;

    [Header("Beam Attack Related")]
    public Transform centerOfRoom;
    public static float beamSpeed = 2f;
    public static float beamTick = .3f;

    [Header("Mine Related Fields")]
    public float numberOfMines = 2;
    public Transform xMin;
    public Transform xMax;
    public Transform zMin;
    public Transform zMax;

    bool antiCampingSpawned = false;
    bool beamSpawned = false;
    bool waveSpawned = false;
    bool homingSpawned = false;
    bool mineSpawned = false;
    bool buffed = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //AntiCampingAttack();
        //BeamAttack();
        //WaveAttack();
        //HomingAttack();
        MineAttack();
        //BuffAndHeal();
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

    void HomingAttack()
    {
        if (!homingSpawned)
        {
            homingSpawned = true;
            Instantiate(homingProjectile, transform.position, transform.rotation);
        }
    }

    void MineAttack() 
    {
        if (!mineSpawned)
        {
            mineSpawned = true;
            float xMin = this.xMin.position.x;
            float xMax = this.xMax.position.x;
            float zMin = this.zMin.position.z;
            float zMax = this.zMax.position.z;
            Vector3 minePos;

            for (int i = 0; i < numberOfMines; i++)
            {
                minePos = new Vector3(Random.Range(xMin, xMax), 0, Random.Range(zMin, zMax));
                Instantiate(minePrefab, minePos, transform.rotation);
            }
        }

        else 
        {
            if (GameObject.FindGameObjectWithTag("Mine") == null) 
            {
                mineSpawned = false;
            }
        }
    }

    void BuffAndHeal() 
    {
        if (!buffed) 
        {
            var EnemyHealth = GetComponent<EnemyHealth>();
            buffed = true;
            BeamDamage = (int)(BeamDamage * buffAmount);
            HomingDamage = (int)(HomingDamage * buffAmount);
            MineDamage = (int)(MineDamage * buffAmount);
            WaveDamage = (int)(WaveDamage * buffAmount);

            EnemyHealth.HealUp(healAmount);
        }
    }
}
