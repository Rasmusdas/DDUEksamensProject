using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{

    public List<Pool> pools;
    public Dictionary<PoolType, Queue<GameObject>> poolDictionary;
    public static ObjectPooling objectPool;
    private void Awake()
    {
        objectPool = this;
    }


    void Start()
    {
        poolDictionary = new Dictionary<PoolType, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject poolObject = Instantiate(pool.prefab);

                poolObject.SetActive(false);

                objectPool.Enqueue(poolObject);
            }

            poolDictionary.Add(pool.type,objectPool);
        }
    }


    public GameObject InstantiateFromPool(PoolType type, Vector3 position)
    {
        if(!poolDictionary.ContainsKey(type))
        {
            throw new System.ArgumentException("Pool type is not defined");
        }
        Queue<GameObject> poolQueue = poolDictionary[type];

        GameObject objFromPool = poolQueue.Dequeue();

        objFromPool.SetActive(true);

        objFromPool.transform.position = position;

        return objFromPool;
    }
}

[System.Serializable]
public class Pool
{
    public PoolType type;
    public GameObject prefab;
    public int size;
}

public enum PoolType
{
    Normal,
    Tree
}
