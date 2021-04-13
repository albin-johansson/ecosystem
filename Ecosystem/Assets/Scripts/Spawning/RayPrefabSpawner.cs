using System;
using Ecosystem.Util;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Ecosystem.Spawning
{
  public sealed class RayPrefabSpawner : MonoBehaviour
  {
    [SerializeField] private Terrain terrain;
    [SerializeField] private GameObject prefab;

    [Tooltip("This directory will be overwritten if objectPool is used")] [SerializeField]
    private Transform directory;

    [SerializeField] private bool useRadius;
    [SerializeField] private float radius;

    [Tooltip("Fill this field if using objectPooling for the spawning gameObject")] [SerializeField]
    private string keyInPool;

    [SerializeField] private float spawnRate;

    private float _elapsedTime;
    private bool _usePool;
    private bool _haveCheckedPool;

    private void Update()
    {
      if (!_haveCheckedPool)
      {
        _usePool = ObjectPoolHandler.instance.isPoolValid(keyInPool);
        _haveCheckedPool = true;
      }

      _elapsedTime += Time.deltaTime;
      if (spawnRate > _elapsedTime)
      {
        return;
      }

      _elapsedTime = 0;

      if (useRadius)
      {
        SpawnInRadius();
      }
      else
      {
        SpawnOnNavMesh();
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
          var spawnedObject = ObjectPoolHandler.instance.GetFromPool(keyInPool);
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