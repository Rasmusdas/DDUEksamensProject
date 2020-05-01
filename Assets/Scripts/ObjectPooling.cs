using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mostly from https://www.youtube.com/watch?v=tdSmKaJvCoA. But with a few changes like tag has been changed to an Enum as tag didn't make much sense for pre defined pools.
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

    /// <summary>
    /// Instantiates from PoolType pool.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="position"></param>
    /// <returns></returns>
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

        objFromPool.GetComponent<IPoolObject>().PoolStart();

        return objFromPool;
    }
}


// Defines what kind of prefabs are generated from the beginning.
[System.Serializable]
public class Pool
{
    public PoolType type;
    public GameObject prefab;
    public int size;
}

// This actually isn't used as all tiles contain all the models just not activated.
public enum PoolType
{
    SmallHouse,
    MediumHouse,
    BigHouse,
    Church,
    Normal,
    Tree,
    
}
