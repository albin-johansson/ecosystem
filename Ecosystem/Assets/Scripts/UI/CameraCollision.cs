using System;
using UnityEngine;

namespace Ecosystem.UI
{
  public class CameraCollision : MonoBehaviour
  {
    public float smoothTime = 0.1f;
    private Transform _transform;

    [Tooltip("The min permitted Height limit between Cam and Terrain")]
    public float minCamTerrainHeight = 0.15f;

    [Tooltip("Raycast height from the Cam to Terrain")]
    public float terrainDetectionHeight = 2f;

    //public float zoomSpeed = 350f;
    private Vector3 velocity = Vector3.zero;
    private bool colliding = false;
    private Collider col;

    public void Update()
    {
      collide();
    }

    void OnTriggerEnter(Collider other)
    {
      col = other;
      colliding = true;
    }

    void OnTriggerExit(Collider other)
    {
//      if (other.gameObject.CompareTag("Terrain"))
      colliding = false;
    }

    //If Camera collides with Terrain, move the camera smoothly above the terrain
    private void collide()
    {
      if (colliding)
      {
        Debug.Log(colliding);
        _transform.position = Vector3.SmoothDamp(_transform.localPosition, new Vector3(0, minCamTerrainHeight, 0),
          ref velocity, smoothTime);
        float moveDown = Input.GetAxis("Mouse ScrollWheel");
        _transform.Translate(new Vector3(0, moveDown) * Time.deltaTime * -zoomSpeed, 0);
      }

      //Check Distance between Terrain-Camera object
      /*
      RaycastHit hit;
      if (Physics.Raycast(_transform.position, -_transform.up, out hit, terrainDetectionHeight))
      {
      }
      */
    }
  }
}