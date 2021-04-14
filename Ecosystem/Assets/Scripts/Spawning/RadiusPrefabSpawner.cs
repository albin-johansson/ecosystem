using UnityEngine;
using Random = UnityEngine.Random;

namespace Ecosystem.Spawning
{
  // Will spawn a given prefab inside a radius if it hits something with a ground tag.
  public sealed class RadiusPrefabSpawner : MonoBehaviour
  {
    [SerializeField] private GameObject prefab;
    [SerializeField] private float radius;

    [SerializeField, Tooltip("How many times an object is spawned, per second")]
    private float spawnRate;

    private float _spawnRateRatio;
    private float _nextSpawnTime;

    private void Start()
    {
      _spawnRateRatio = 1.0f / spawnRate;
    }

    private void Update()
    {
      if (Time.unscaledTime > _nextSpawnTime)
      {
        var distance = Random.Range(0, radius);
        var dir = Random.insideUnitCircle;

        var transformPosition = transform.position;
        var position = transformPosition + distance * new Vector3(dir.x, 0, dir.y);
        var origin = position + new Vector3(0, transformPosition.y + 5, 0);

        if (!Physics.Raycast(origin, Vector3.down, out var hit, 200.0f))
        {
          return;
        }

        if (hit.transform.CompareTag("Terrain"))
        {
          Instantiate(prefab, hit.point, Quaternion.identity);
        }

        _nextSpawnTime = Time.unscaledTime + _spawnRateRatio;
      }
    }
  }
}