using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Will spawn the given prefab where the user clicks. 
public class SpawnOnClick : MonoBehaviour
{
  [SerializeField] private GameObject prefab;

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Vector3 mouseScreenPosition = Input.mousePosition;
      Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

      if (Physics.Raycast(ray, out RaycastHit hitInfo))
      {
        SpawnAtPosition(hitInfo.point);
      }
    }
  }

  private void SpawnAtPosition(Vector3 spawnPosition)
  {
    GameObject food = Instantiate(prefab, spawnPosition, Quaternion.identity);
  }
}