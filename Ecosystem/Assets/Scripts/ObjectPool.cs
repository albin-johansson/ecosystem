using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Ecosystem
{
  public class ObjectPool : MonoBehaviour
  {
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;


    #region Singleton

    public static ObjectPool instance;

    private void Awake()
    {
      instance = this;
    }

    #endregion

    private void Start()
    {
      poolDictionary = new Dictionary<string, Queue<GameObject>>();

      foreach (var pool in pools)
      {
        var objectPool = new Queue<GameObject>();

        for (var i = 0; i < pool.size; i++)
        {
          var objectToPool = Instantiate(pool.prefab);
          objectToPool.SetActive(false);
          objectPool.Enqueue(objectToPool);
        }

        poolDictionary.Add(pool.tag, objectPool);
      }
    }

    public GameObject GetFromPool(string poolTag)
    {
      var wantedPool = poolDictionary[poolTag];
      if (wantedPool.Count > 1)
      {
        return wantedPool.Dequeue();
      }
      else
      {
        return Instantiate(wantedPool.Peek());
      }
    }

    public void ReturnToPool(string poolTag, GameObject objectToReturn)
    {
      var wantedPool = poolDictionary[poolTag];
      objectToReturn.SetActive(false);
      wantedPool.Enqueue(objectToReturn);
    }
  }
}