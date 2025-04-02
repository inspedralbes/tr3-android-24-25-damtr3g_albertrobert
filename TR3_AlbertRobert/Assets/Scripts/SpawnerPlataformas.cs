using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SpawnerPlataformas : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject prefab2;
    
    // Valores por defecto
    private float spawnRate = 3f;
    private float minX = -36f;
    private float maxX = 36f;
    private float alternativeSpawnChance = 0.2f;

    void Start()
    {
        StartCoroutine(LoadConfigFromServer());
    }

    IEnumerator LoadConfigFromServer()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://localhost:4000/config/config"))
        {
            yield return webRequest.SendWebRequest();
            
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                ConfigData config = JsonUtility.FromJson<ConfigData>(webRequest.downloadHandler.text);
                
                spawnRate = config.spawnRate;
                minX = config.minX;
                maxX = config.maxX;
                alternativeSpawnChance = config.alternativeSpawnChance;
            }
            else
            {
                Debug.LogError("Error al cargar configuración: " + webRequest.error);
            }
        }
        
        StartSpawning();
    }

    void StartSpawning()
    {
        InvokeRepeating("SpawnPrefab", 0f, spawnRate);
    }

    void SpawnPrefab()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), transform.position.y, transform.position.z);
        GameObject prefabToSpawn = Random.value < alternativeSpawnChance ? prefab2 : prefab1;
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }

    [System.Serializable]
    private class ConfigData
    {
        public float spawnRate;
        public float minX;
        public float maxX;
        public float alternativeSpawnChance;
    }
}