using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    public float attackCooldown = 5;
    public float animationCooldown = 2;
    public AudioClip attackSFX;
    float attackTime = 0;
    Animator anim;

    [Header("Boss Attack Damage Values")]
    public static int AntiCampingProjectileDamage = 10;
    public static int BeamDamage = 5;
    public static int WaveDamage = 20;
    public static int HomingDamage = 10;
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

    [Header("Stun stuff :)")]
    public float stunDuration = 3.0f;
    bool stunned = false;
    float timer = 0;

    bool antiCampingSpawned = false;
    bool beamSpawned = false;
    bool waveSpawned = false;
    bool homingSpawned = false;
    bool mineSpawned = false;
    bool buffed = false;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(target);

        if (stunned) {
            timer += Time.deltaTime;
            if (timer >= stunDuration) {
                stunned = false;
                timer = 0;
            } else {
                Debug.Log("Stunned");
                return;
            }
        }

        attackTime += Time.deltaTime;
        if (attackTime >= animationCooldown) {
            anim.SetInteger("animState", 0);
        }

        if (attackTime >= attackCooldown) {
            attackTime = 0;
            anim.SetInteger("animState", 2);
            AudioSource.PlayClipAtPoint(attackSFX, Camera.main.transform.position);

            int attack = Random.Range(0, 6);
            switch (attack) {
                case 0:
                    AntiCampingAttack();
                    break;
                case 1:
                    print("beam");
                    BeamAttack();
                    break;
                case 2:
                    print("wave");
                    WaveAttack();
                    break;
                case 3:
                    print("homing");
                    HomingAttack();
                    break;
                case 4:
                    print("mine");
                    MineAttack();
                    break;
                case 5:
                    BuffAndHeal();
                    break;
            }
        }
    }

    void AntiCampingAttack()
    {
        for (int ii = 0; ii < 4; ++ii) {
            antiCampingProjectile.GetComponent<AntiCampingProjectileBehavior>().CreateProjectile(ii * 2);
        }

        /*
        if (!antiCampingSpawned)
        {
            antiCampingSpawned = true;
            for (int ii = 0; ii < 4; ++ii)
            {
                antiCampingProjectile.GetComponent<AntiCampingProjectileBehavior>().CreateProjectile(ii * 2);
            }
        }*/
    }

    void BeamAttack()
    {
        beamSpawned = true;
        Instantiate(beamProjectile, transform.position + Vector3.up,
            Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z + 90));
        Instantiate(beamProjectile, transform.position + Vector3.up,
Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 45, transform.rotation.eulerAngles.z + 90));
        Instantiate(beamProjectile, transform.position + Vector3.up,
Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 135, transform.rotation.eulerAngles.z + 90));
        Instantiate(beamProjectile, transform.position + Vector3.up,
            Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 90));
        /*
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
        }*/
    }

    void WaveAttack()
    {
        Instantiate(waveProjectile, centerOfRoom.position, centerOfRoom.rotation);
        /*
        if (!waveSpawned)
        {
            waveSpawned = true;
            Instantiate(waveProjectile, centerOfRoom.position, centerOfRoom.rotation);
        }*/
    }

    void HomingAttack()
    {
        Instantiate(homingProjectile, transform.position + Vector3.up, transform.rotation);
        /*
        if (!homingSpawned)
        {
            homingSpawned = true;
            Instantiate(homingProjectile, transform.position + Vector3.up, transform.rotation);
        }*/
    }

    void MineAttack() 
    {
        mineSpawned = true;
        float xMin = this.xMin.position.x;
        float xMax = this.xMax.position.x;
        float zMin = this.zMin.position.z;
        float zMax = this.zMax.position.z;
        Vector3 minePos;

        for (int i = 0; i < numberOfMines; i++) {
            minePos = new Vector3(Random.Range(xMin, xMax), 0, Random.Range(zMin, zMax));
            Instantiate(minePrefab, minePos, transform.rotation);
        }

        /*
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
        }*/
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
    public void Stun() {
        stunned = true;
    }
}
