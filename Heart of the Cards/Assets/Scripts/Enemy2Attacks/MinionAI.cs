using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour {
    public enum FSMStates {
        Idle, Patrol, Chase, Attack
    }

    public FSMStates currentState;
    public float attackDistance = 5;
    public float enemySpeed = 5;
    public float chaseDistance = 10;
    public GameObject player;
    public GameObject[] spellProjectiles;

    public GameObject wandTip;
    public float shootRate = 2;
    public GameObject deadVFX;

    GameObject[] wanderPoints;
    Vector3 nextDestination;
    int currentDestinationIndex = 0;
    float distanceToPlayer;
    float elapsedTime = 0;

    bool isDead;

    Animator anim;
    NavMeshAgent agent;

    public Transform enemyEyes;
    public float fieldOfView = 45f;

    // Start is called before the first frame update
    void Start() {
        Initialize();
    }

    private void Initialize() {
        wanderPoints = GameObject.FindGameObjectsWithTag("Waypoint");
        player = GameObject.FindGameObjectWithTag("Player");

        currentState = FSMStates.Patrol;
        isDead = false;

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        FindNextPoint();
    }

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        if (isDead) {
            return;
        }
        if (agent == null) {
            agent = GetComponent<NavMeshAgent>();
        }
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState) {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
        }

        EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth.currentHealth <= 0) {
            isDead = true;
        }
        elapsedTime += Time.deltaTime;
    }

    void UpdatePatrolState() {
        anim.SetInteger("animState", 3);
        FaceTarget(nextDestination);

        if (Vector3.Distance(transform.position, nextDestination) < 2) {
            FindNextPoint();
        } else if (distanceToPlayer <= chaseDistance && IsPlayerInClearFOV()) {
            currentState = FSMStates.Chase;
        }

        agent.speed = 3.5f;
        agent.stoppingDistance = 0;
        agent.SetDestination(nextDestination);
    }

    void UpdateChaseState() {
        anim.SetInteger("animState", 3);
        nextDestination = player.transform.position;
        FaceTarget(nextDestination);

        if (distanceToPlayer <= attackDistance) {
            currentState = FSMStates.Attack;
        } else if (distanceToPlayer > chaseDistance) {
            FindNextPoint();
            currentState = FSMStates.Patrol;
        }

        agent.speed = 5;
        agent.stoppingDistance = attackDistance;
        agent.SetDestination(nextDestination);
    }

    void UpdateAttackState() {
        anim.SetInteger("animState", 2);
        nextDestination = player.transform.position;
        FaceTarget(nextDestination);

        if (distanceToPlayer <= attackDistance) {
            currentState = FSMStates.Attack;
        } else if (distanceToPlayer > attackDistance && distanceToPlayer < chaseDistance) {
            currentState = FSMStates.Chase;
        } else if (distanceToPlayer > chaseDistance) {
            currentState = FSMStates.Patrol;
        }

        EnemySpellCast();
    }

    void FindNextPoint() {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;

        agent.SetDestination(nextDestination);
    }

    void FaceTarget(Vector3 target) {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    void EnemySpellCast() {
        if (elapsedTime >= shootRate && !isDead) {
            Invoke("SpellCasting", 0);
            elapsedTime = 0;
        }
    }

    void SpellCasting() {
        GameObject spellProjectile = spellProjectiles[Random.Range(0, spellProjectiles.Length)];

        Instantiate(spellProjectile, wandTip.transform.position, wandTip.transform.rotation);
    }

    private void OnDrawGizmos() {
        // attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        // chase
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Vector3 frontRayPoint = enemyEyes.position + (enemyEyes.forward * chaseDistance);
        Vector3 leftRayPoint = Quaternion.Euler(0, fieldOfView * .5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -fieldOfView * .5f, 0) * frontRayPoint;

        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.cyan);
        Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.yellow);
    }

    bool IsPlayerInClearFOV() {
        RaycastHit hit;
        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        if (Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView
                && Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance)) {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }
}