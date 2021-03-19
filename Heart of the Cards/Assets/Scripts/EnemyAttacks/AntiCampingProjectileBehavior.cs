using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiCampingProjectileBehavior : MonoBehaviour
{

    public GameObject[] waypoints;
    public float projectileSpeed = 10f;

    Transform target;
    public int currIndex;

    public void CreateProjectile(int waypointIndex)
    {
        var projectile = Instantiate(gameObject, waypoints[waypointIndex].transform.position, waypoints[waypointIndex].transform.rotation);
        projectile.GetComponent<AntiCampingProjectileBehavior>().currIndex = waypointIndex;
        projectile.GetComponent<AntiCampingProjectileBehavior>().FindNextPoint();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("Set Waypoints For Anti Camping Projectile");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target.position) <= 1f)
        {
            FindNextPoint();
        }

        float step = projectileSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    public void FindNextPoint()
    {
        currIndex = (currIndex + 1) % waypoints.Length;
        target = waypoints[currIndex].transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.playerHealth.TakeDamage(EnemyAttacks.AntiCampingProjectileDamage);
        }
    }
}
