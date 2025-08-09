using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [Header("Prefabs & Spawn")]
    public List<GameObject> obstaclePrefabs; // at least 4 types
    public float spawnX = 12f;           // X position to spawn obstacles
    public float minGap = 3f;            // min gap in world units
    public float maxGap = 6f;            // max gap in world units
    public Transform obstacleParent;

    [Header("Cleanup")]
    public float despawnX = -14f;        // delete when offscreen

    private float lastSpawnX;

    private List<GameObject> activeObstacles = new List<GameObject>();

    private void Start()
    {
        lastSpawnX = spawnX;
        SpawnInitialObstacles();
    }

    private void Update()
    {
        if (activeObstacles.Count == 0 || activeObstacles[activeObstacles.Count - 1].transform.position.x < spawnX - minGap)
        {
            SpawnRandomObstacle();
        }
        CleanUpOffscreen();
    }

    private void SpawnInitialObstacles()
    {
        // Spawn one at start
        SpawnRandomObstacle();
    }

    private void SpawnRandomObstacle()
    {
        if (obstaclePrefabs.Count == 0) return;

        // Pick a random prefab
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];

        // Pick gap distance
        float gap = Random.Range(minGap, maxGap);

        // New spawn position based on last obstacle X
        Vector3 spawnPos = new Vector3(spawnX, prefab.transform.position.y, 0f);

        // Instantiate
        GameObject go = Instantiate(prefab, spawnPos, Quaternion.identity, obstacleParent);
        activeObstacles.Add(go);

        // Update last spawn
        lastSpawnX = spawnPos.x;
    }

    private void CleanUpOffscreen()
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            var o = activeObstacles[i];
            if (o == null) { activeObstacles.RemoveAt(i); continue; }
            if (o.transform.position.x < despawnX)
            {
                Destroy(o);
                activeObstacles.RemoveAt(i);
            }
        }
    }
}
