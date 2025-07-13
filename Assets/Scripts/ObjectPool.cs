using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PoolItem
{
    public PoolTypeEnum type;              // "platform", "coin", "obstacle"
    public GameObject prefab;
    public int poolSize = 10;
}

public class ObjectPool : MonoBehaviour
{
    public PoolItem[] items;

    private Dictionary<PoolTypeEnum, Queue<GameObject>> pools = new();

    void Awake()
    {
        foreach (PoolItem item in items)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();

            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject obj = Instantiate(item.prefab, transform.position, Quaternion.identity);
                obj.SetActive(false);
                poolQueue.Enqueue(obj);
            }

            pools[item.type] = poolQueue;
        }
    }

    public GameObject GetFromPool(PoolTypeEnum type, Vector3 position)
    {
        if (!pools.ContainsKey(type))
        {
            Debug.LogError("type " + type + " not found!");
            return null;
        }

        GameObject obj = pools[type].Dequeue();
        obj.transform.position = position;
        obj.SetActive(true);
        pools[type].Enqueue(obj);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        PoolType poolType = obj.GetComponent<PoolType>();
        if (poolType == null)
        {
            Debug.LogWarning("Returned object doesn't have PoolType component!");
            return;
        }
        PoolTypeEnum type = poolType.type;

        obj.SetActive(false);
        obj.transform.SetParent(transform);
        if (pools.ContainsKey(type))
        {
            pools[type].Enqueue(obj);
        }
        else
        {
            Debug.LogWarning("Trying to return to unknown pool: " + type);
        }
    }

}
