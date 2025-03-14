using UnityEngine;

public class SpawnerPlataformas : MonoBehaviour
{
    public GameObject prefab1; // Primer prefab
    public GameObject prefab2; // Segundo prefab (el que puede aparecer aleatoriamente)
    public float spawnRate = 2f; // Tiempo entre cada spawn
    public float minX = -5f, maxX = 5f; // Rango de posiciones en X
    public float alternativeSpawnChance = 0.2f; // Probabilidad de que aparezca prefab2 (20%)
    
    void Start()
    {
        InvokeRepeating("SpawnPrefab", 0f, spawnRate);
    }

    void SpawnPrefab()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), transform.position.y, transform.position.z);
        GameObject prefabToSpawn = Random.value < alternativeSpawnChance ? prefab2 : prefab1;
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }
}
