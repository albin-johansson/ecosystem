using UnityEngine;

// Will spawn a given prefab inside the given terrain if it hits something with a ground tag.
public sealed class RayPrefabSpawner : MonoBehaviour
{
  [SerializeField] private GameObject prefab;
  [SerializeField] private Terrain terrain;
  [SerializeField] private float rate;
  private float _elapsedTime; 

  private void Update()
  {
    _elapsedTime += Time.deltaTime;
    if (rate < _elapsedTime)
    {
      _elapsedTime = 0;
      
      var terrainData = terrain.terrainData;
      var xPos = Random.Range(-terrainData.bounds.extents.x, terrainData.bounds.extents.x);
      var zPos = Random.Range(-terrainData.bounds.extents.z, terrainData.bounds.extents.z);
      
      var position = terrainData.bounds.center + new Vector3(xPos, 0, zPos);
      var height = terrain.SampleHeight(terrainData.bounds.center + new Vector3(xPos, 0, zPos)) + 10;

      if (!Physics.Raycast(position + new Vector3(0, height, 0), Vector3.down, out var hit, 200.0f)) return;
      if (hit.transform.CompareTag("Ground"))
      {
        Instantiate(prefab, hit.point, Quaternion.identity);
      }
    }
  }
}