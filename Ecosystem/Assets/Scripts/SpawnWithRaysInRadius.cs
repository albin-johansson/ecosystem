using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Will spawn a given prefab inside a radius if it hits something with a ground tag.
public class SpawnWithRaysInRadius : MonoBehaviour
{
    
    [SerializeField] private GameObject prefab;
    [SerializeField] private float rate;
    [SerializeField] private float radius;
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
            Vector3 position = transform.position + distance * new Vector3(dir.x, 0, dir.y);
            
            RaycastHit hit;
            
            if (Physics.Raycast (position + new Vector3(0, transform.position.y + 5, 0), Vector3.down, out hit, 200.0f)) 
            {
                if (hit.transform.CompareTag("Ground")) 
                {
                    Instantiate (prefab, hit.point, Quaternion.identity); 
                }
            }
        }
        
    }
}
