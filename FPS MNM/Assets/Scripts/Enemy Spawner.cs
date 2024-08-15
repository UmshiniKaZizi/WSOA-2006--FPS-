using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{   
    public GameObject Enemyprefab;
   // public GameObject SpawnerPrefab;
    public float SpawnInterval;
    public int  MaxEnemies = 5 ;
    public Transform[] SpawnPoints ;
    public GameObject spawnPointPrefab; // Reference to the spawn point prefab

    public int CurrentEnemyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            Transform spawnPoint = Instantiate(spawnPointPrefab, SpawnPoints[i].position, SpawnPoints[i].rotation).transform;
            SpawnPoints[i] = spawnPoint; // Assign the instantiated spawn point to the array
        }
        StartCoroutine(SpawnEnemies());
    }
    public IEnumerator SpawnEnemies()
    {   while (true)
        {
            yield return new WaitForSeconds(SpawnInterval);

            if (CurrentEnemyCount < MaxEnemies)

            {
                Transform SpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
                //Instantiate(Enemyprefab, SpawnPoint.position, SpawnPoint.rotation);
                GameObject newEnemy = Instantiate(Enemyprefab, SpawnPoint.position, SpawnPoint.rotation);
                newEnemy.GetComponent<Enemy>().spawner = this; // Pass the reference to the Enemy script
                CurrentEnemyCount++;

            }
        }
    }

    public void EnemyDestroyed()
    {
        CurrentEnemyCount--;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
