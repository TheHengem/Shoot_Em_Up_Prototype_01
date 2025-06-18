using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro.Examples;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject[] spawnPrefabs; // List of prefabs that can spawn
    public float spawnInterval = 20f; // Time interval between spawns
    public int minSpawnCount = 3;     // Minimum number of objects to spawn
    public int maxSpawnCount = 5;     // Maximum number of objects to spawn (inclusive)
    public float spawnDistance = 50f; // Distance from the player at which to spawn objects

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        // Initial delay if needed, otherwise immediately spawn on start
        yield return new WaitForSeconds(spawnInterval);

        while (true)
        {
            SpawnObjects();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnObjects()
    {
        minSpawnCount = minSpawnCount + 1;
        maxSpawnCount = maxSpawnCount + 1;
        // Determine how many objects to spawn this interval.
        // Note: Random.Range with ints is min inclusive and max exclusive, so add 1 to include maxSpawnCount.
        int countToSpawn = Random.Range(minSpawnCount, maxSpawnCount + 1);

        for (int i = 0; i < countToSpawn; i++)
        {
            // Choose a random angle (in radians) for the spawn position.
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);
            Vector3 spawnDirection = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f);

            // Calculate the spawn position relative to the player.
            Vector3 spawnPosition = transform.position + spawnDirection * spawnDistance;

            // Randomly select one prefab from the provided list.
            if (spawnPrefabs.Length > 0)
            {
                int prefabIndex = Random.Range(0, spawnPrefabs.Length);
                GameObject prefabToSpawn = spawnPrefabs[prefabIndex];

                // Instantiate the selected prefab at the spawn position with default rotation.
                Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("No prefabs assigned to spawnPrefabs!");
                break;
            }
        }
    }
}
