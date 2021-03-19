using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int projectileDamage = 10;
    public static GameObject player;
    public static PlayerHealth playerHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }
    /*

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
