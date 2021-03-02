using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 100f;
    public Image reticleImage;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        damage = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            GameObject projectile = Instantiate(projectilePrefab, 
                transform.position + transform.forward, transform.rotation) as GameObject;

            var projectileDamage = projectile.GetComponent<ProjectileBehavior>();
            projectileDamage.damage = damage;

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

            projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);
        }
    }
}
