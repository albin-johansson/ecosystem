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
    private static int _walkable;
    public static ObjectPool instance;
    private Transform _poolTransform;

    private void Awake()
    {
      instance = this;
    }

    private void Start()
    {
      _poolTransform = transform;
      _walkable = 1 << NavMesh.GetAreaFromName("Walkable");
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

    private void ResetObjectComponents(GameObject objectToReset)
    {
      var navMeshAgent = objectToReset.GetComponent<NavMeshAgent>();
      navMeshAgent.isStopped = true;
      navMeshAgent.ResetPath();
      objectToReset.transform.parent = _poolTransform;
      objectToReset.SetActive(false);
    }
  }
}