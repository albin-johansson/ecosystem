using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

namespace Ecosystem.Spawning
{
  public class ObjectPool : MonoBehaviour
  {
    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> _poolDictionary;


    public static ObjectPool instance;

    private void Awake()
    {
      instance = this;
    }

    private void Start()
    {
      _poolDictionary = new Dictionary<string, Queue<GameObject>>();

      foreach (var pool in pools)
      {
        var objectPool = new Queue<GameObject>();

        for (var i = 0; i < pool.size; i++)
        {
          var objectToPool = Instantiate(pool.prefab);
          objectToPool.SetActive(false);
          objectPool.Enqueue(objectToPool);
        }

        _poolDictionary.Add(pool.tag, objectPool);
      }
    }

    public GameObject GetFromPool(string poolTag)
    {
      var wantedPool = _poolDictionary[poolTag];
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
      var wantedPool = _poolDictionary[poolTag];
      ResetObjectComponents(objectToReturn);
      wantedPool.Enqueue(objectToReturn);
    }

    private void ResetObjectComponents(GameObject objectToReset)
    {
      //TODO: Replace with OnEnable and OnDisable in movement controller and stateController
      var navMeshAgent = objectToReset.GetComponent<NavMeshAgent>();
      navMeshAgent.isStopped = true;
      navMeshAgent.ResetPath();
      objectToReset.SetActive(false);
    }
  }
}