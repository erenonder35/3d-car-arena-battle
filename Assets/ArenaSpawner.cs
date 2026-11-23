using System.Collections;
using UnityEngine;

public class ArenaSpawner : MonoBehaviour
{
    [Header("Üretilecek Objeler (Prefabs)")]
    public GameObject goldPrefab;
    public GameObject bombPrefab;

    [Header("Alan Ayarlarý")]
    public float spawnRadius = 60f;
    public float ySpawnHeight = 2f;

    [Header("Süre Ayarlarý")]
    public float goldSpawnInterval = 4f;
    public float bombMinTime = 15f;
    public float bombMaxTime = 30f;

    void Start()
    {
        StartCoroutine(SpawnGoldRoutine());
        StartCoroutine(SpawnBombRoutine());
    }

    IEnumerator SpawnGoldRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(goldSpawnInterval);
            SpawnItem(goldPrefab);
        }
    }

    IEnumerator SpawnBombRoutine()
    {
        while (true)
        {
            float randomWaitTime = Random.Range(bombMinTime, bombMaxTime);
            yield return new WaitForSeconds(randomWaitTime);
            SpawnItem(bombPrefab);
        }
    }

    void SpawnItem(GameObject itemToSpawn)
    {
        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPoint.x, ySpawnHeight, randomPoint.y);
        spawnPosition += transform.position;
        Instantiate(itemToSpawn, spawnPosition, itemToSpawn.transform.rotation);
    }
}