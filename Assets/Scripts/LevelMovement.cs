using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelMovement : MonoBehaviour
{
    public float despawnZ = -50f;
    public SpeedMoveManager speedManager;
    public float speed;
    
    private List<GameObject> movingPlatforms = new List<GameObject>();

    void Start()
    {
        if (speedManager == null)
        {
            speedManager = FindAnyObjectByType<SpeedMoveManager>();
        }
    }

    public void RegisterPlatform(GameObject platform)
    {
        if (!movingPlatforms.Contains(platform))
        {
            movingPlatforms.Add(platform);
        }
    }

    void Update()
    {
        speed = speedManager != null ? speedManager.GetCurrentSpeed() : 45f;
        Vector3 moveDelta = Vector3.back * speed * Time.deltaTime;

        for (int i = 0; i < movingPlatforms.Count; i++)
        {
            GameObject platform = movingPlatforms[i];
            platform.transform.Translate(moveDelta);

            if (platform.transform.position.z < despawnZ)
            {
                PlatformSpawner spawner = FindAnyObjectByType<PlatformSpawner>();
                spawner.HandlePlatformDespawn(platform);
                break;
            }
        }
    }
}
