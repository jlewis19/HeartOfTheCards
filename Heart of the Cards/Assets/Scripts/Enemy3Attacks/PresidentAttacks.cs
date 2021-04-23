using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresidentAttacks : MonoBehaviour
{
    public float attackCooldown = 5;
    public float animationCooldown = 2;
    public AudioClip attackSFX;
    float attackTime = 0;

    [Header("Boss Attack Damage Values")]
    public static int homingDamage = 10;
    public static int stunDamage = 5;
    public static float stunProjDuration = 2f;
    public static int spiralDamage = 10;

    [Header("Projectile Prefabs")]
    public GameObject homingPrefab;
    public GameObject stunPrefab;
    public GameObject beamPrefab;
    public GameObject spiralPrefab;

    [Header("Position Fields")]
    public Transform xMin;
    public Transform xMax;
    public Transform zMin;
    public Transform zMax;

    [Header("Stun stuff :)")]
    public float stunDuration = 3.0f;
    bool stunned = false;
    float timer = 0;

    [Header("Spiral Attack Info")]
    public float spiralDuration = 5f;
    public Transform frontSpawnPoint;
    public Transform backSpawnPoint;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        frontSpawnPoint.RotateAround(this.transform.position, Vector3.up, 90f * Time.deltaTime);
        backSpawnPoint.RotateAround(this.transform.position, Vector3.up, 90f * Time.deltaTime);

        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(target);
        if (stunned)
        {
            timer += Time.deltaTime;
            if (timer >= stunDuration)
            {
                stunned = false;
                timer = 0;
            }
            else
            {
                Debug.Log("Stunned");
                return;
            }
        }
        attackTime += Time.deltaTime;
        if (attackTime >= attackCooldown)
        {
            attackTime = 0;
            //anim.SetInteger("animState", 2);
            //AudioSource.PlayClipAtPoint(attackSFX, Camera.main.transform.position);

            int attack = Random.Range(0, 4);
            switch (attack)
            {
                case 0:
                    HomingProjectile();
                    break;
                case 1:
                    StunProjectile();
                    break;
                case 2:
                    FloorAttack();
                    break;
                case 3:
                    SpiralAttack();
                    break;
            }
        }
    }

    void HomingProjectile()
    {
        Instantiate(homingPrefab, transform.position + Vector3.up, transform.rotation);
    }

    void StunProjectile()
    {
        Instantiate(stunPrefab, transform.position + Vector3.up, transform.rotation);
    }
    void FloorAttack() {
        for (int i = 0; i < 10; i++) {
            Invoke("InstantiateBeam", (float) i / 2);
        }
    }

    void SpiralAttack()
    {
        

        for (float i = 0; i < spiralDuration; i++) 
        {
            Invoke("InstantiateSpiral", i / 8);
        }
    }

    void InstantiateBeam() {
        Vector3 beamPos = new Vector3(Random.Range(this.xMin.position.x, this.xMax.position.x), 0,
            Random.Range(this.zMin.position.z, this.zMax.position.z));
        GameObject beam = Instantiate(beamPrefab, beamPos, transform.rotation);
        beam.transform.localScale = new Vector3(beam.transform.localScale.x * 5f, beam.transform.localScale.y * 1f, beam.transform.localScale.z * 5f);
    }

    void InstantiateSpiral() 
    {
        Instantiate(spiralPrefab, frontSpawnPoint.position + Vector3.forward, transform.rotation);
        Instantiate(spiralPrefab, backSpawnPoint.position + Vector3.back, transform.rotation);
    }
}
