using Ecosystem.Util;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Ecosystem.Spawning
{
  public sealed class RayPrefabSpawner : MonoBehaviour
  {
    [SerializeField] private Terrain terrain;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform directory; // This will be overwritten if object pooling is used
    [SerializeField] private bool useRadius;
    [SerializeField] private float radius;
    [SerializeField] private float spawnRate;

    private string _keyInPool;
    private float _elapsedTime;
    private bool _usePool;

    private void Start()
    {
      _keyInPool = prefab.tag;
      _usePool = ObjectPoolHandler.Instance.HasPool(_keyInPool);
    }

    private void Update()
    {
      _elapsedTime += Time.deltaTime;
      if (_elapsedTime > spawnRate)
      {
        if (useRadius)
        {
          SpawnInRadius();
        }
        else
        {
          SpawnOnNavMesh();
        }

        _elapsedTime = 0;
      }
    }

    private void SpawnInRadius()
    {
      var dir = Random.insideUnitCircle * radius;
      var position = transform.position + new Vector3(dir.x, 0, dir.y);
      Spawn(position, radius);
    }

    private void Spawn(Vector3 position, float wantedRadius)
    {
      if (NavMesh.SamplePosition(position, out var hit, wantedRadius, Terrains.Walkable))
      {
        if (_usePool)
        {
          var spawnedObject = ObjectPoolHandler.Instance.Construct(_keyInPool);
          spawnedObject.transform.position = hit.position;
          spawnedObject.transform.parent = directory;
          spawnedObject.SetActive(true);
        }
        else
        {
          Instantiate(prefab, hit.position, Quaternion.identity, directory);
        }
      }
    }

    private void SpawnOnNavMesh()
    {
      var terrainData = terrain.terrainData;
      var xPos = Random.Range(0, terrainData.bounds.max.x);
      var zPos = Random.Range(0, terrainData.bounds.max.z);
      var position = terrain.transform.position + new Vector3(xPos, 0, zPos);

      Spawn(position, terrainData.bounds.max.y);
    }
  }
}