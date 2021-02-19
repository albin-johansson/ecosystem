using UnityEngine;

namespace Ecosystem
{
  // Will spawn the given prefab where the user clicks. 
  public sealed class ClickPrefabSpawner : MonoBehaviour
  {
    [SerializeField] private GameObject prefab;
    private Camera _mainCamera;

    private void Start()
    {
      _mainCamera = Camera.main;
    }

    private void Update()
    {
      if (!Input.GetMouseButtonDown(0))
      {
        return;
      }

      if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo))
      {
        SpawnAtPosition(hitInfo.point);
      }
    }

    private void SpawnAtPosition(Vector3 spawnPosition)
    {
      Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
  }
}