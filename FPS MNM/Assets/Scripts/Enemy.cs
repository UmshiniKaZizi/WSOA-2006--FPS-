using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public EnemySpawner spawner;  // Reference to the spawner
    public float health = 100f;
    public float pushForce = 5f;
    public float pushDuration = 0.5f;
    public float detectionRadius = 8000f;  // Radius within which the enemy detects the player
    public float patrolRadius = 15000f;     // Radius within which the enemy patrols
    public float patrolWaitTime = 0.1f;    // Time to wait at each patrol point

    private NavMeshAgent enemy;
    private Rigidbody enemyRb;
    private GameObject player;
    private Vector3 patrolPoint;
    private bool isBeingPushed = false;
    private bool isChasingPlayer = false;
    private float patrolWaitCounter;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemyRb = GetComponent<Rigidbody>();
        enemy = GetComponent<NavMeshAgent>();

        SetNewPatrolPoint();
        patrolWaitCounter = patrolWaitTime;
    }

    // Method to deal damage to the enemy
    public void enemydamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            spawner.EnemyDestroyed();  // Notify the spawner when this enemy is destroyed
            Destroy(gameObject);
        }
    }

    // Method to reset enemy health
    public void ResetHealth()
    {
        health = 100f;
    }

    // OnTriggerEnter for collision with player or bullets
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            enemydamage(10f); // Adjust damage if needed
        }

        if (other.CompareTag("Player"))
        {
            PlayerAttributes playerAttributes = other.GetComponent<PlayerAttributes>();
            if (playerAttributes != null)
            {
                playerAttributes.TakeDamage(10f);

                // Apply pushback force
                Vector3 pushDirection = (transform.position - other.transform.position).normalized;
                pushDirection.y = 0;

                StartCoroutine(PushBack(pushDirection));
            }
        }
    }

    // Coroutine for applying pushback
    private IEnumerator PushBack(Vector3 pushDirection)
    {
        enemy.isStopped = true;
        isBeingPushed = true;

        float elapsedTime = 0f;
        while (elapsedTime < pushDuration)
        {
            enemyRb.MovePosition(transform.position + pushDirection * pushForce * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isBeingPushed = false;
        enemy.isStopped = false;
    }

    void Update()
    {
        if (isBeingPushed) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Debug the distance to ensure it's calculating correctly
        Debug.Log("Distance to player: " + distanceToPlayer);

        // Check if player is within detection radius
        if (distanceToPlayer <= detectionRadius)
        {
            Debug.Log("Player detected within range");
            isChasingPlayer = true;
            enemy.SetDestination(player.transform.position);
        }
        else
        {
            isChasingPlayer = false;
            Patrol();
        }
    }

    // Patrol logic for moving between random points on the NavMesh
    private void Patrol()
    {
        if (!enemy.pathPending && enemy.remainingDistance < 0.5f)
        {
            patrolWaitCounter -= Time.deltaTime;

            if (patrolWaitCounter <= 0)
            {
                SetNewPatrolPoint();
                patrolWaitCounter = patrolWaitTime;
            }
        }

        enemy.SetDestination(patrolPoint);
    }

    // Set a new patrol point within the patrol radius
    private void SetNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolPoint = hit.position;
        }
    }
}
