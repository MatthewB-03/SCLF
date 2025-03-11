using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pools a number of specified prefabs
/// </summary>
public class scp_ObjectPooler : scp_Singleton<scp_ObjectPooler>
{
    [SerializeField] string[] poolNames;
    [SerializeField] GameObject[] prefabsToPool;
    [SerializeField] int[] initialPoolSizes;
    List<GameObject>[] pools;

    // Awake is called when the script instance is loaded
    protected override void Awake()
    {
        base.Awake();
        pools = new List<GameObject>[poolNames.Length];
        FillPools();
    }

    // Called when a new scene is loaded
    private void OnLevelWasLoaded(int level)
    {
        FillPools();
    }

    // Fills each pool with it's specified objects
    void FillPools()
    {
        for (int poolIndex = 0; poolIndex < pools.Length; poolIndex++)
        {
            pools[poolIndex] = new List<GameObject>();

            for (int i = 0; i < initialPoolSizes[poolIndex]; i++)
            {
                AddObjectToPool(poolIndex);
            }
        }
    }

    // Adds one more object to the pool
    GameObject AddObjectToPool(int poolIndex)
    {
        GameObject item = Instantiate(prefabsToPool[poolIndex], transform.position, Quaternion.identity);
        pools[poolIndex].Add(item);
        item.SetActive(false);

        return item;
    }

    // Returns the index of the pool name, or -1 if it does not exist
    int GetPoolIndex(string poolName)
    {
        int poolIndex = 0;

        foreach(string name in poolNames)
        {
            if (poolName == name)
            {
                return poolIndex;
            }
            poolIndex++;
        }

        return -1;
    }
    
    /// <summary>
    /// Returns an object from the specified pool, will return null if the pool does not exist
    /// </summary>
    /// <param name="poolName">The name of the object pool</param>
    /// <returns></returns>
    public GameObject GetObjectFromPool(string poolName)
    {
        GameObject returnObject = null;

        int poolIndex = GetPoolIndex(poolName);
        if (poolIndex != -1)
        {
            // Get an inactive object from the pool
            foreach (GameObject poolObject in pools[poolIndex])
            {
                if (!poolObject.activeInHierarchy)
                {
                    returnObject = poolObject;
                }
            }

            // If no inactive objects are available, add one more to use
            if(returnObject == null)
            {
                returnObject = AddObjectToPool(poolIndex);
            }

            returnObject.SetActive(true);
        }

        return returnObject;
    }
}
