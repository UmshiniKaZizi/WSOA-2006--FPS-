using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    NavMeshAgent enemy;
    Rigidbody enemyRb;
    GameObject player;

    public float health = 100f;
    public float pushForce = 5f;
    public float pushDuration = 0.5f; // How long the push lastsz
    public EnemySpawner spawner;

    private bool isBeingPushed = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemyRb = GetComponent<Rigidbody>();
    }

    public void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
    }

    public void enemydamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            spawner.EnemyDestroyed();
            Destroy(gameObject);
        }
    }

    public void ResetHealth()
    {
        health = 100f;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
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

    private IEnumerator PushBack(Vector3 pushDirection)
    {
        // Temporarily stop the NavMeshAgent and apply manual movement
        enemy.isStopped = true;
        isBeingPushed = true;

        float elapsedTime = 0f;
        while (elapsedTime < pushDuration)
        {
            // Move the enemy manually along the push direction
            enemyRb.MovePosition(transform.position + pushDirection * pushForce * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Re-enable the NavMeshAgent after the push
        isBeingPushed = false;
        enemy.isStopped = false;
    }

    void Update()
    {
        if (!isBeingPushed)
        {
            enemy.SetDestination(player.transform.position); // Chase the player smoothly
        }
    }
}
