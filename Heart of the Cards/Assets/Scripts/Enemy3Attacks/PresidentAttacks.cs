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

    [Header("Projectile Prefabs")]
    public GameObject homingPrefab;
    public GameObject stunPrefab;

    [Header("Stun stuff :)")]
    public float stunDuration = 3.0f;
    bool stunned = false;
    float timer = 0;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
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
        if (attackTime >= attackCooldown)
        {
            attackTime = 0;
            //anim.SetInteger("animState", 2);
            //AudioSource.PlayClipAtPoint(attackSFX, Camera.main.transform.position);

            int attack = Random.Range(0, 2);
            switch (attack)
            {
                case 0:
                    HomingProjectile();
                    break;
                case 1:
                    StunProjectile();
                    break;
                case 2:
                    
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
}
