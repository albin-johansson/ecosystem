using System;
using UnityEngine;

namespace Ecosystem.UI
{
  public sealed class CameraController : MonoBehaviour
  {
    [SerializeField] private float speed = 8;

    private float _x;
    private float _y;

    private Vector3 _rotateValue;

    private Transform _transform;
    private Transform _trackedTarget;
    private bool _track = false;
    private float distance = 20;
    private bool col = false;
    private Rigidbody rb;

    private void Start()
    {
      rb = GetComponent<Rigidbody>();
      _transform = rb.transform;
    }

    private void Update()
    {
      //Stop tracking with "Q"
      if (Input.GetKey(KeyCode.Q))
      {
        _track = false;
      }

      //Select animal to follow with mouse button
      if (Input.GetKey(KeyCode.Mouse0))
      {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
          if (!hit.transform.tag.Equals("Terrain"))
          {
            _trackedTarget = hit.transform;
            _track = true;
            Debug.Log(hit.transform.gameObject.name);
          }
        }
      }

      //Follow the tracked animal
      if (_track)
      {
        //Zoom with mouse wheel
        distance += Input.mouseScrollDelta.y;
        distance = Math.Max(distance, 5);

        //follow
        transform.position = _trackedTarget.position + Vector3.up * distance;
        //TODO: when should always be looking at target?
        //transform.LookAt(_trackedTarget);
      }
      //Move around with "WASD"
      else
      {
        Translate();
      }

      //TODO: how to best handle rotate(). always or sometimes?
      Rotate();

      //Exit application
      if (Input.GetKeyUp(KeyCode.Escape))
      {
        Application.Quit();
      }

      if (rb.velocity.magnitude > speed)
      {
        rb.velocity = rb.velocity.normalized * speed;
      }

      //Check if under map. 
      /*
       //SampleHeight gives a value under the terrain.
      float currentTerrainHeight = Terrain.activeTerrain.SampleHeight(_transform.position);
      if (_transform.position.y < currentTerrainHeight)
        _transform.position = new Vector3(_transform.position.x, currentTerrainHeight+1, _transform.position.z);
        */
      /*
      Ray r = new Ray(_transform.position, Vector3.down); //Camera.main.transform.position + Vector3.down;
      RaycastHit h;
      if (Physics.Raycast(r, out h, 0.01f))
      {
        //CheckCameraCollision(h.point);
        if (h.transform.tag.Equals("Terrain"))
        {
          _transform.position = _transform.position + new Vector3(0, 0.02f, 0);
        }
      }
      
      if (col)
      {
        _transform.position += new Vector3(0, 0.5f, 0);
      }
      */
    }


    /*
    private float offset = 1.0f;

    private void CheckCameraCollision(Vector3 targetPosition)
    {
      Ray ray = new Ray(targetPosition, Vector3.down); //_transform.position - targetPosition);
      RaycastHit hit = new RaycastHit();
      if (Physics.Raycast(ray, out hit, (_transform.position - targetPosition).magnitude))
      {
        if (hit.distance > offset)
          _transform.position = ray.GetPoint(hit.distance - offset);
        else
          _transform.position = targetPosition;
      }
    }
    

    public void OnCollisionEnter()
    {
      Debug.Log("Collided with something");
      //col = true;
      //_transform.position = new Vector3(_transform.position.x, _transform.position.y, _transform.position.z);
    }

    public void OnCollisionExit()
    {
      col = false;
    }
    */

    private Vector3 forward;
    private Vector3 back;
    private Vector3 right;
    private Vector3 left;

    private void Translate()
    {
      if (Input.GetKey(KeyCode.W))
      {
        rb.velocity += 100 * speed * Time.deltaTime * _transform.forward;
        //_transform.position += speed * Time.deltaTime * _transform.forward;
      }

      if (Input.GetKey(KeyCode.S))
      {
        rb.velocity -= 100 * speed * Time.deltaTime * _transform.forward;
        //_transform.position -= speed * Time.deltaTime * _transform.forward;
      }

      if (Input.GetKey(KeyCode.D))
      {
        rb.velocity += 100 * speed * Time.deltaTime * _transform.right;
        //_transform.position += speed * Time.deltaTime * _transform.right;
      }

      if (Input.GetKey(KeyCode.A))
      {
        rb.velocity -= 100 * speed * Time.deltaTime * _transform.right;
        //_transform.position -= speed * Time.deltaTime * _transform.right;
      }

      //Key up
      if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) ||
          Input.GetKeyUp(KeyCode.A))
      {
        rb.velocity = Vector3.zero;
        //_transform.position += speed * Time.deltaTime * _transform.forward;
      }
    }

    private void Rotate()
    {
      _y = Input.GetAxis("Mouse X");
      _x = Input.GetAxis("Mouse Y");
      _rotateValue = new Vector3(_x, _y * -1, 0);
      _transform.eulerAngles -= _rotateValue;
    }
  }
}