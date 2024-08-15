using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{   NavMeshAgent enemy;
    GameObject player;
    public float health = 100f;
   
    public EnemySpawner spawner;
    // Start is called before the first frame update
    void Start()
    {
        //enemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
    }

    public void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
    }
    void enemydamage(float damage)
    {
        health -= damage;
        if (health < 0) 
        {
            spawner.EnemyDestroyed();
            Destroy(gameObject);
           
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            enemydamage(50f);
            Destroy(other.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
       enemy.SetDestination(player.transform.position);
    }
}
