using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public Slider healthBar;
    protected Animator anim;
    public AudioClip hitSFX;

    public int currentHealth;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
        healthBar.maxValue = startingHealth;
        SetHealthBar();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (currentHealth <= 0) {
            if (gameObject.CompareTag("Enemy")) {
                FindObjectOfType<LevelManager>().EnemyDies();
            }
            anim.SetInteger("animState", 1);
            Destroy(gameObject, 3);
        }
    }

    public virtual void takeDamage(int amount) {
        AudioSource.PlayClipAtPoint(hitSFX, transform.position);
        currentHealth -= amount;
        SetHealthBar();
        Debug.Log("Current enemy health: " + currentHealth);
    }

    public void HealUp(int amount) 
    {
        if (currentHealth / startingHealth > .5f) 
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
            SetHealthBar();
            Debug.Log("Enemy recovered and now has " + currentHealth);
        }
    }

    private void SetHealthBar()
    {
        healthBar.value = currentHealth;
    }
}
