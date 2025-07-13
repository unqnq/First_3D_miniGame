using System;
using UnityEngine;

public class LevelMovement : MonoBehaviour
{
    public float moveSpeed = 45f;
    public float despawnZ = 0f;
    public Action<GameObject> OnDespawn;

    private SpeedMoveManager speedManager;

    void Start()
    {
        speedManager = FindFirstObjectByType<SpeedMoveManager>();
        if (speedManager == null)
        {
            Debug.LogError("SpeedManager not found in scene!");
        }
    }
    void Update()
    {
        moveSpeed = speedManager != null ? speedManager.GetCurrentSpeed() : 45f;

        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        if (transform.position.z < despawnZ)
        {
            OnDespawn?.Invoke(gameObject);
        }
    }
}
