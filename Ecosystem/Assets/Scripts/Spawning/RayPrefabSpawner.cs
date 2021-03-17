using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Ecosystem.Spawning
{
  // Will spawn a given prefab inside the given terrain if it hits something with a ground tag.
  public sealed class RayPrefabSpawner : MonoBehaviour
  {
    [SerializeField] private Transform directory;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Terrain terrain;
    [SerializeField] private float rate;
    private float _elapsedTime;
    private int _walkable;
    

    private void Start()
    {
    _walkable = 1 << NavMesh.GetAreaFromName("Walkable");
    }

    private void Update()
    {
      _elapsedTime += Time.deltaTime;
      if (rate < _elapsedTime)
      {
        _elapsedTime = 0;

        var terrainData = terrain.terrainData;
        var xPos = Random.Range(-terrainData.bounds.extents.x, terrainData.bounds.extents.x);
        var zPos = Random.Range(-terrainData.bounds.extents.z, terrainData.bounds.extents.z);

        var position = transform.position + new Vector3(xPos, 0, zPos);
        
        if (NavMesh.SamplePosition(position, out var hit, float.PositiveInfinity, _walkable))
        {
          print("instantiating");
          Instantiate(prefab, hit.position, Quaternion.identity, directory);
        }
      }
    }
  }
}