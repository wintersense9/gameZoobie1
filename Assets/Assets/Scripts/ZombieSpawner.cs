using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Spawns zombies at increasing rates as the episode progresses.
/// Keeps track of active zombies for SurvivorAgent observation queries.
/// </summary>
public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject zombiePrefab;
    public float initialSpawnInterval = 5f;
    public float minimumSpawnInterval = 1.5f;
    public float spawnRange           = 8f;
    public int   maxZombies           = 20;

    [Header("Difficulty Scaling")]
    [Tooltip("Spawn interval decreases by this much each wave")]
    public float intervalDecrement = 0.3f;

    // ── Active zombie list (queried by SurvivorAgent) ──────────────────────
    public List<GameObject> ActiveZombies { get; } = new();

    private float currentInterval;
    private float timer;

    void Start()
    {
        currentInterval = initialSpawnInterval;
        timer = currentInterval;
    }

    void Update()
    {
        // Clean up destroyed entries
        ActiveZombies.RemoveAll(z => z == null);

        if (ActiveZombies.Count >= maxZombies) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnZombie();
            // Gradually decrease interval (harder over time)
            currentInterval = Mathf.Max(minimumSpawnInterval, currentInterval - intervalDecrement);
            timer = currentInterval;
        }
    }

    public void SpawnZombie()
    {
        // Spawn outside a safe radius around the agent so it isn't instant-killed
        Vector3 pos;
        int attempts = 0;
        do
        {
            pos = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                1f,
                Random.Range(-spawnRange, spawnRange)
            );
            attempts++;
        }
        while (pos.magnitude < 3f && attempts < 10);  // keep ≥3 units from centre

        GameObject z = Instantiate(zombiePrefab, pos, Quaternion.identity);
        ActiveZombies.Add(z);
    }

    /// <summary>Destroy all zombies (called on episode reset).</summary>
    public void ClearAllZombies()
    {
        foreach (var z in ActiveZombies)
            if (z != null) Destroy(z);
        ActiveZombies.Clear();

        currentInterval = initialSpawnInterval;
        timer = currentInterval;
    }
}
