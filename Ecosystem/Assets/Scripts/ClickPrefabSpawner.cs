using System;
using UnityEngine;

// Will spawn the given prefab where the user clicks. 
public sealed class ClickPrefabSpawner : MonoBehaviour
{
  [SerializeField] private GameObject prefab;
  private Camera camera1;

  private void Start()
  {
    camera1 = Camera.main;
  }

  private void Update()
  {
    if (!Input.GetMouseButtonDown(0)) return;

    if (Physics.Raycast(camera1.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
    {
      SpawnAtPosition(hitInfo.point);
    }
  }

  private void SpawnAtPosition(Vector3 spawnPosition)
  {
    Instantiate(prefab, spawnPosition, Quaternion.identity);
  }
}