using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    #region Attributes

    // initial size for our pool of prefab instances
    private const int defaultPoolSize = 20;

    // set limit to number of instances in our pool (max bullets)
    private const int maxPoolSize = 50;

    public static bool expandPool = true;

    // dictionary with string key to point to a pool (list of prefab instances)
    public static Dictionary<string, List<GameObject>>
        objectPool = new Dictionary<string, List<GameObject>>();

    #endregion

    #region Pool checks

    // check dictionary objectPool - return true if a pool list for a given prefab exists
    private static bool PoolExistForPrefab(string prefabPath)
    {
        return objectPool.ContainsKey(prefabPath);
    }

    // return true if GameObject is not active
    private static bool IsAvailableForReuse(GameObject gameObject)
    {
        return !gameObject.activeSelf;
    }

    #endregion

    #region Pool Creation

    // adds new instance to a prefab pool and returns instance
    private static GameObject ExpandPool(string prefabPath, List<GameObject> pool)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        GameObject instance = Object.Instantiate<GameObject>(prefab);
        pool.Add(instance);
        return instance;
    }


    // add instances of a prefab and return pool list
    public static List<GameObject> CreatePool(string prefabPath, int count) {
        if (count <= 0) count = 1;

        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        List<GameObject> pool = new List<GameObject>();
        for (int i=0; i<count; i++)
        {
            GameObject instance = Object.Instantiate<GameObject>(prefab);
            instance.SetActive(false);
            pool.Add(instance);
        }

        // add pool to dictionary
        objectPool.Add(prefabPath, pool);

        return pool;
    }

    #endregion


    #region Pool Retrieve Prefab Instance


    // return an available instance from a prefab pool
    public static GameObject GetPoolObject(string prefabPath, int poolSize = defaultPoolSize)
    {
        // if no pool in dictionary
        if ( !PoolExistForPrefab(prefabPath)) {
            return CreateAndRetrievePoolObject(prefabPath, poolSize);
        }

        // else get pool
        var pool = objectPool[prefabPath];

        // get 1st available instance, else add new instance to pool and return the instance
        GameObject instance = FindFirstAvailable(pool);
        return (instance != null) ? instance : ExpandPool(prefabPath, pool);

    }


    // initialize prefab pool and return 1st available pool instance
    private static GameObject CreateAndRetrievePoolObject(string prefabPath, int poolSize = defaultPoolSize)
    {
        CreatePool(prefabPath, poolSize);
        return GetPoolObject(prefabPath);
    }

     
    // return first NON active instance from a pool list
    private static GameObject FindFirstAvailable(List<GameObject> pool)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (IsAvailableForReuse(pool[i]))
            {
                return pool[i];
            }
        }
        return null;
    }

    #endregion


}

