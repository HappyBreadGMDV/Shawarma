using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;   // что спавнить
    public Transform spawnPoint;       // где спавнить

    public void Spawn()
    {
        if (prefabToSpawn == null || spawnPoint == null)
        {
            Debug.LogError("Prefab or spawn point not assigned!", this);
            return;
        }
        Instantiate(prefabToSpawn, spawnPoint.position,spawnPoint.rotation);
    }
}