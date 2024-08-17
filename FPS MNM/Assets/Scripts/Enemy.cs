using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent enemy;
    GameObject player;
    public float health = 100f;

    public EnemySpawner spawner;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
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
    }

    void Update()
    {
        enemy.SetDestination(player.transform.position);
    }
}
