using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarAttacks : MonoBehaviour
{
    public float attackCooldown = 5;
    public float animationCooldown = 2;
    public AudioClip attackSFX;
    float attackTime = 0;

    [Header("Boss Attack Damage Values")]
    public static int RotatingProj = 10;
    public static int MeleeDamage = 30;
    public static int WaveDamage = 10;

    [Header("Projectile Prefabs")]
    public GameObject rotatingProjPrefab;
    public GameObject meleePrefab;
    public GameObject waveProjectile;
    public GameObject minionPrefab;

    [Header("Minion Related Fields")]
    public float minionNumber = 2;
    public Transform xMin;
    public Transform xMax;
    public Transform zMin;
    public Transform zMax;

    [Header("Melee Fields")]
    public float attackDistance = 5;
    public float movementSpeed = 3;

    Animator anim;
    GameObject player;
    NavMeshAgent agent;


    bool goonsSpawned;
    bool waveSpawned;
    bool dashSpawned;
    bool rotationSpawned;

    float distanceToPlayer;
    bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) {
            FindObjectOfType<LevelManager>().EnemyDies();
            return;
        }

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(target);

        attackTime += Time.deltaTime;
        if (attackTime >= animationCooldown)
        {
            anim.SetInteger("animState", 0);
        }

        if (attackTime >= attackCooldown)
        {
            attackTime = 0;
            anim.SetInteger("animState", 2);

            int attack = Random.Range(0, 4);
            switch (attack)
            {
                case 0:
                    print("Rotation");
                    RotationAttack();
                    break;
                case 1:
                    print("Melee");
                    MeleeAttack();
                    AudioSource.PlayClipAtPoint(attackSFX, Camera.main.transform.position);
                    break;
                case 2:
                    print("Leap");
                    WaveAttack();
                    AudioSource.PlayClipAtPoint(attackSFX, Camera.main.transform.position);
                    break;
                case 3:
                    print("Minion");
                    SpawnGoons();
                    break;
            }
        }

        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health.currentHealth <= 0) {
            isDead = true;
        }
    }

    void RotationAttack()
    {
        //rotationSpawned = true;
        GameObject projectile = Instantiate(rotatingProjPrefab, transform.position + new Vector3(3, 2f, 0),
            Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90,
            transform.rotation.eulerAngles.z + 90));
        projectile.transform.SetParent(gameObject.transform);
        projectile = Instantiate(rotatingProjPrefab, transform.position + new Vector3(-3, 2f, 0),
            Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180,
            transform.rotation.eulerAngles.z + 180));
        projectile.transform.SetParent(gameObject.transform);
        projectile = Instantiate(rotatingProjPrefab, transform.position + new Vector3(0, 2f, 3),
            Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 270,
            transform.rotation.eulerAngles.z + 270));
        projectile.transform.SetParent(gameObject.transform);
        projectile = Instantiate(rotatingProjPrefab, transform.position + new Vector3(0, 2f, -3),
            Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z));
        projectile.transform.SetParent(gameObject.transform);
    }

    void MeleeAttack()
    {
        if (distanceToPlayer <= attackDistance)
        {
            FaceTarget(player.transform.position);
            Instantiate(meleePrefab, transform.position + Vector3.forward + Vector3.up, transform.rotation);
            anim.SetInteger("animState", 2);

        }
        else 
        {
            agent.SetDestination(player.transform.position);
            anim.SetInteger("animState", 3);
            Invoke("MeleeAttack", 3);
        }
    }

    void WaveAttack()
    {
        if (!waveSpawned)
        {
            //waveSpawned = true;
            Instantiate(waveProjectile, transform.position + Vector3.up, transform.rotation);
        }
    }

    void SpawnGoons() 
    {
        if (!goonsSpawned)
        {
            //goonsSpawned = true;
            float xMin = this.xMin.position.x;
            float xMax = this.xMax.position.x;
            float zMin = this.zMin.position.z;
            float zMax = this.zMax.position.z;
            Vector3 minePos;

            for (int i = 0; i < minionNumber; i++)
            {
                minePos = new Vector3(Random.Range(xMin, xMax), 0, Random.Range(zMin, zMax));
                Instantiate(minionPrefab, minePos, transform.rotation);
            }
        }/*

        else
        {
            if (GameObject.FindGameObjectWithTag("Enemy") < mini)
            {
                goonsSpawned = false;
            }
        }*/
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionTarget = (target - transform.position).normalized;
        directionTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }
}
