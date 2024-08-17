using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemyprefab;
    public float SpawnInterval;
    public int MaxEnemies = 5;
    public Transform[] SpawnPoints;
    public GameObject spawnPointPrefab;

    public int CurrentEnemyCount = 0;

    void Start()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            Transform spawnPoint = Instantiate(spawnPointPrefab, SpawnPoints[i].position, SpawnPoints[i].rotation).transform;
            SpawnPoints[i] = spawnPoint;
        }
        StartCoroutine(SpawnEnemies());
    }

    public IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(SpawnInterval);

            if (CurrentEnemyCount < MaxEnemies)
            {
                Transform spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
                GameObject newEnemy = Instantiate(Enemyprefab, spawnPoint.position, spawnPoint.rotation);
                newEnemy.GetComponent<Enemy>().spawner = this;
                CurrentEnemyCount++;
            }
        }
    }

    public void EnemyDestroyed()
    {
        CurrentEnemyCount--;
    }

    public void ResetEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.ResetHealth();
        }
    }

    void Update()
    {
        // Call ResetEnemies() as needed
    }
}
