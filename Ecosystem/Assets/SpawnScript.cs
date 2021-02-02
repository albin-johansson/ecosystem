using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnScript : MonoBehaviour
{

    [SerializeField] private GameObject prefab;
    [SerializeField] private Terrain terrain;
    [SerializeField] private float rate;
    private float elapsedTime;
    
    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                SpawnFoodAtPosition(hitInfo.point);
            }
        } */
        elapsedTime += Time.deltaTime;
        if (rate < elapsedTime)
        {
            elapsedTime = 0;
            var terrainData = terrain.terrainData;
            float xPos = Random.Range(-terrainData.bounds.extents.x, terrainData.bounds.extents.x);
            float zPos = Random.Range(-terrainData.bounds.extents.z, terrainData.bounds.extents.z);
        
            float height = terrain.SampleHeight(terrainData.bounds.center + new Vector3(xPos, 0, zPos));

            Vector3 worldspacePos = terrainData.bounds.center + new Vector3(xPos, height, zPos);
            Instantiate(prefab, worldspacePos, Quaternion.identity);
        }

    }
/*
    private void SpawnFoodAtPosition(Vector3 spawnPosition)
    {
        GameObject food = Instantiate(_foodPrefab, spawnPosition, Quaternion.identity);
    }
    */
}
