using UnityEngine;

// Will spawn a given prefab inside a radius if it hits something with a ground tag.
public sealed class RayRadiusPrefabSpawner : MonoBehaviour
{
  [SerializeField] private GameObject prefab;
  [SerializeField] private float rate;
  [SerializeField] private float radius;
  private float _elapsedTime;
  
  private void Update()
  {
    _elapsedTime += Time.deltaTime;
    if (rate < _elapsedTime)
    {
      _elapsedTime = 0;
      
      var distance = Random.Range(0, radius);
      var dir = Random.insideUnitCircle;
      var position = transform.position + distance * new Vector3(dir.x, 0, dir.y);

      RaycastHit hit;

      if (Physics.Raycast(position + new Vector3(0, transform.position.y + 5, 0), Vector3.down, out hit, 200.0f))
      {
        if (hit.transform.CompareTag("Ground"))
        {
          Instantiate(prefab, hit.point, Quaternion.identity);
        }
      }
    }
  }
}