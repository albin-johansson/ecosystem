using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Will spawn a given prefab inside the given terrain if it hits something with a ground tag.
public class SpawnWithRays : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Terrain terrain;
    [SerializeField] private float rate;
    private float elapsedTime;
    private bool hitGround = false;

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (rate < elapsedTime)
        {
            elapsedTime = 0;
            
            RaycastHit hit;
            var terrainData = terrain.terrainData;
            float xPos = Random.Range(-terrainData.bounds.extents.x, terrainData.bounds.extents.x);
            float zPos = Random.Range(-terrainData.bounds.extents.z, terrainData.bounds.extents.z);
            
            
            Vector3 position = terrainData.bounds.center + new Vector3(xPos, 0, zPos);
            float height = terrain.SampleHeight(terrainData.bounds.center + new Vector3(xPos, 0, zPos)) + 10;
            
            if (Physics.Raycast (position + new Vector3(0, height, 0), Vector3.down, out hit, 200.0f)) 
            { 
                if (hit.transform.CompareTag("Ground"))
                {
                    Instantiate (prefab, hit.point, Quaternion.identity);
                }
            } else {
                Debug.Log ("there seems to be no ground at this position");
            }

            hitGround = false;
        }
    }
}
