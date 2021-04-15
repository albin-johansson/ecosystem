using System.Collections.Generic;
using Ecosystem.Util;
using UnityEngine;
using UnityEngine.AI;

namespace Ecosystem.Spawning
{
  public sealed class ObjectPoolHandler : MonoBehaviour
  {
    [SerializeField] private List<Pool> pools;

    public static ObjectPoolHandler Instance;

    private readonly Dictionary<string, Queue<GameObject>> _poolMap = new Dictionary<string, Queue<GameObject>>();
    private Transform _transform;

    private void Awake()
    {
      Instance = this;
      
      _transform = transform;

      foreach (var pool in pools)
      {
        var objectPool = new Queue<GameObject>();

        for (var i = 0; i < pool.size; ++i)
        {
          if (NavMesh.SamplePosition(_transform.position, out var hit, 100, Terrains.Walkable))
          {
            var obj = Instantiate(pool.prefab, hit.position, _transform.rotation, _transform);
            obj.SetActive(false);

            objectPool.Enqueue(obj);
          }
        }

        _poolMap.Add(pool.prefab.tag, objectPool);
      }
    }

    public bool HasPool(string key)
    {
      return _poolMap.ContainsKey(key);
    }

    public GameObject Construct(string key)
    {
      var pool = _poolMap[key];
      return pool.Count > 1 ? pool.Dequeue() : Instantiate(pool.Peek());
    }

    public void ReturnToPool(string key, GameObject objectToReturn)
    {
      var wantedPool = _poolMap[key];
      ResetObject(objectToReturn);
      wantedPool.Enqueue(objectToReturn);
    }

    // Returns the object to the specified pool if it exists, otherwise the object is simply destroyed
    public void ReturnOrDestroy(string key, GameObject objectToReturn)
    {
      if (HasPool(key))
      {
        ReturnToPool(key, objectToReturn);
      }
      else
      {
        Destroy(objectToReturn);
      }
    }

    /// <summary>
    ///   Resets select values and variables in components that are connected to the game object. Also moves inactive
    ///   game objects to the pool in the hierarchy.
    /// </summary>
    private void ResetObject(GameObject objectToReset)
    {
      if (TryGetComponent(out NavMeshAgent agent))
      {
        agent.isStopped = true;
        agent.ResetPath();
      }

      objectToReset.transform.parent = _transform;
      objectToReset.SetActive(false);
    }
  }
}