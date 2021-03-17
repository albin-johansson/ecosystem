using System.Collections.Generic;
using Ecosystem.Util;
using UnityEngine;
using UnityEngine.AI;

namespace Ecosystem.Spawning
{
  public class ObjectPoolHandler : MonoBehaviour
  {
    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> _poolDictionary;
    private static int _walkable;
    public static ObjectPoolHandler instance;
    private Transform _poolTransform;

    /// <summary>
    /// This makes the PoolHandler a 'Singleton'. This makes it so that the same instance of the class is used.
    /// </summary>
    private void Awake()
    {
      instance = this;
    }

    /// <summary>
    /// This function will initiate all pools and Instantiate the gameObjects in the pools.
    /// </summary>
    private void Start()
    {
      _poolTransform = transform;
      _walkable = Terrains.Walkable;
      _poolDictionary = new Dictionary<string, Queue<GameObject>>();

      foreach (var pool in pools)
      {
        var objectPool = new Queue<GameObject>();

        for (var i = 0; i < pool.size; i++)
        {
          NavMesh.SamplePosition(_poolTransform.position, out var hit, float.PositiveInfinity, _walkable);
          var objectToPool = Instantiate(pool.prefab, hit.position, _poolTransform.rotation, _poolTransform);
          objectToPool.SetActive(false);
          objectPool.Enqueue(objectToPool);
        }

        _poolDictionary.Add(pool.key, objectPool);
      }
    }

    public bool isPoolValid(string poolKey)
    {
      return _poolDictionary.ContainsKey(poolKey);
    }

    public GameObject GetFromPool(string poolKey)
    {
      var wantedPool = _poolDictionary[poolKey];
      if (wantedPool.Count > 1)
      {
        return wantedPool.Dequeue();
      }
      else
      {
        return Instantiate(wantedPool.Peek());
      }
    }


    public void ReturnToPool(string poolKey, GameObject objectToReturn)
    {
      var wantedPool = _poolDictionary[poolKey];
      ResetObjectComponents(objectToReturn);
      wantedPool.Enqueue(objectToReturn);
    }


    /// <summary>
    /// Resets values and variables in components that is connected to the GameObject.
    /// Also moves inactive gameObjects to the pool in the hierarchy.
    /// </summary>
    private void ResetObjectComponents(GameObject objectToReset)
    {
      if (TryGetComponent<NavMeshAgent>(out var navMeshAgent))
      {
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
      }

      objectToReset.transform.parent = _poolTransform;
      objectToReset.SetActive(false);
    }
  }
}