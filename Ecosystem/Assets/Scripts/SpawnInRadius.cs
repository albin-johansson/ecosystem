using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Will spawn prefab inside the radius, taking no account to what is below
public class SpawnInRadius : MonoBehaviour
{
  [SerializeField] private GameObject prefab;
  [SerializeField] private float radius;
  [SerializeField] private float rate;
  private float elapsedTime;

  // Update is called once per frame
  void Update()
  {
    elapsedTime += Time.deltaTime;
    if (rate < elapsedTime)
    {
      elapsedTime = 0;
      float distance = Random.Range(0, radius);
      Vector2 dir = Random.insideUnitCircle;
      Vector3 worldSpacePosition = transform.position + distance * new Vector3(dir.x, 0, dir.y);
      Instantiate(prefab, worldSpacePosition, Quaternion.identity);
    }
  }
}