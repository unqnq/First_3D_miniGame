using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public ObjectPool pool;
    public float platformLength = 50f;
    public Transform player;
    public int spawnAheadCount = 5;

    public float spawnZ = 0f;
    private List<GameObject> activePlatforms = new List<GameObject>();

    void Start()
    {
        player = GameObject.Find("Player").transform;
        pool = FindAnyObjectByType<ObjectPool>();

        for (int i = 0; i < spawnAheadCount; i++)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        spawnZ = activePlatforms.Count * platformLength;
        Vector3 spawnPos = new Vector3(0, 0, spawnZ);
        GameObject platform = pool.GetFromPool(PoolTypeEnum.Platform, spawnPos);

        LevelMovement level = FindAnyObjectByType<LevelMovement>();
        level.RegisterPlatform(platform);
        activePlatforms.Add(platform);
    }

    public void HandlePlatformDespawn(GameObject platform)
    {
        if (activePlatforms.Contains(platform))
        {
            activePlatforms.Remove(platform);
            spawnZ = activePlatforms[activePlatforms.Count - 1].transform.position.z;
            Vector3 newPos = new Vector3(0, 0, spawnZ+platformLength);
            platform.transform.position = newPos;
            activePlatforms.Add(platform);

            SpawnObstaclesAndCoins(platform.transform);
        }
    }

    void SpawnObstaclesAndCoins(Transform platform)
    {
        for (int i = platform.childCount - 1; i >= 0; i--)
        {
            Transform child = platform.GetChild(i);
            pool.ReturnToPool(child.gameObject);
        }

        float[] lanes = { -6f, -3f, 0f, 3f, 6f };
        int itemCount = Random.Range(1, 6);

        for (int i = 0; i < itemCount; i++)
        {
            float x = lanes[Random.Range(0, lanes.Length)];
            float z = Random.Range(3f, platformLength - 3f);
            Vector3 spawnPos = platform.position + new Vector3(x, 1f, z);

            PoolTypeEnum type = (Random.value > 0.8f) ? PoolTypeEnum.Coin : PoolTypeEnum.Obstacle;
            GameObject obj = pool.GetFromPool(type, spawnPos);
            obj.transform.SetParent(platform);

        }
    }
}
