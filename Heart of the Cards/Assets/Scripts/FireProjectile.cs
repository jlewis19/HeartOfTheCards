using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 100f;
    public Image reticleImage;
    public int damage = 10;
    public AudioClip throwSFX;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        if (MouseSensitivityManager.active)
        {
            reticleImage.enabled = false;
        }
        else
        {
            reticleImage.enabled = true;
        }

        if (Input.GetButtonDown("Fire1") && gameObject.GetComponentInParent<CardController>().hasHand && !MouseSensitivityManager.active) {
            AudioSource.PlayClipAtPoint(throwSFX, transform.position);
            GameObject projectile = Instantiate(projectilePrefab, 
                transform.position + transform.forward, transform.rotation) as GameObject;

            var projectileDamage = projectile.GetComponent<ProjectileBehavior>();
            projectileDamage.damage = damage;

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

            projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);

            gameObject.GetComponentInParent<CardController>().ThrowProjectile();
        }
    }
}
