using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemyprefab;
    public float SpawnInterval = 3f;
    public int MaxEnemies = 5;
    public Transform[] SpawnPoints;
    public GameObject spawnPointPrefab;
    private int spawnCount = 0;
    public int CurrentEnemyCount = 0;
    private bool spawningEnabled = true;

    void Start()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            Transform spawnPoint = Instantiate(spawnPointPrefab, SpawnPoints[i].position, SpawnPoints[i].rotation).transform;
            SpawnPoints[i] = spawnPoint;
        }
        StartCoroutine(SpawnEnemies());
    }

    // Coroutine to spawn enemies
    private IEnumerator SpawnEnemies()
    {
        while (spawningEnabled)
        {
            yield return new WaitForSeconds(SpawnInterval);

            if (CurrentEnemyCount < MaxEnemies)
            {
                Transform spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
                GameObject newEnemy = Instantiate(Enemyprefab, spawnPoint.position, spawnPoint.rotation);
                newEnemy.GetComponent<Enemy>().spawner = this;  // Assign spawner reference
                CurrentEnemyCount++;
                spawnCount++;

                if (spawnCount == 20)
                {
                    StopSpawning();
                }
            }
        }
    }

    // Method to call when an enemy is destroyed
    public void EnemyDestroyed()
    {
        CurrentEnemyCount--;
    }

    // Method to stop enemy spawning
    public void StopSpawning()
    {
        spawningEnabled = false;
        Debug.Log("Spawning has been stopped.");
    }

    // Method to reset all enemy health
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
        // Optionally place any update logic here
    }
}
