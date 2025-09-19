using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//实现对象池
public class PoolManager : SingletonMonoBehavior<PoolManager>
{
    [System.Serializable]
    public struct Pool
    {
        public int poolId;
        public int size;
        public GameObject prefab;

    }
    public Transform poolParent;
    public List<Pool> pools;
    private Dictionary<int, Queue<GameObject>> poolDictionary;

    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        InitPoolDictionary();
    }

    private void InitPoolDictionary()
    {
        poolDictionary = new Dictionary<int, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            CreatePool(pool);
        }
    }
    private void CreatePool(Pool pool)
    {
        string prefabName = pool.prefab.name;
        GameObject prefabParent = new GameObject(prefabName + "Parent");
        prefabParent.transform.SetParent(poolParent);

        if (!poolDictionary.ContainsKey(pool.poolId))
        {
            poolDictionary.Add(pool.poolId, new Queue<GameObject>());
            for (int i = 0; i < pool.size; i++)
            {
                GameObject prefab = Instantiate(pool.prefab, prefabParent.transform);
                prefab.transform.name = pool.prefab.name;
                prefab.SetActive(false);
                poolDictionary[pool.poolId].Enqueue(prefab);
            }
        }
    }

    public GameObject GetGameObject(int poolId) //获取对象池物品
    {
        if (!poolDictionary.ContainsKey(poolId))
        {
            Debug.Log("对象池id不存在");
            return null;
        }

        GameObject ob = poolDictionary[poolId].Dequeue();
        ob.SetActive(true);
        return ob;
    }

    public void ReturnToPool(int poolId, GameObject ob) //返回对象池
    {
        if (!poolDictionary.ContainsKey(poolId))
        {
            Debug.Log("对象池不存在");
            return;
        }

        ob.SetActive(false);
        poolDictionary[poolId].Enqueue(ob);
    }
}
