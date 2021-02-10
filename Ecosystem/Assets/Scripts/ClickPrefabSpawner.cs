using UnityEngine;

// Will spawn the given prefab where the user clicks. 
public sealed class ClickPrefabSpawner : MonoBehaviour
{
  [SerializeField] private GameObject prefab;
  
  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      var mouseScreenPosition = Input.mousePosition;
      var ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

      if (Physics.Raycast(ray, out RaycastHit hitInfo))
      {
        SpawnAtPosition(hitInfo.point);
      }
    }
  }

  private void SpawnAtPosition(Vector3 spawnPosition)
  {
    Instantiate(prefab, spawnPosition, Quaternion.identity);
  }
}