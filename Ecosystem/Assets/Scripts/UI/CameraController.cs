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
    [SerializeField] private Rigidbody _rigidbody;
    private Transform _trackedTarget;
    private bool _track = false;
    private float distance = 20; //TODO: add support for changing distance, scroll wheel?
    private bool isColliding = false;
    private float frozenY;

    private void Start()
    {
      _transform = transform; // Cache reference for performance reasons
    }

    private void LateUpdate()
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

      //Check if under map. 
      
      /*
      Ray r = new Ray(_transform.position, Vector3.down); //Camera.main.transform.position + Vector3.down;
      RaycastHit h;

      if (Physics.Raycast(r, out h, 1))
      {
        if (h.transform.tag.Equals("Terrain"))
        {
          _transform.position = _transform.position + Vector3.up;
        }
      }
      */
    }

    public void OnTriggerEnter(Collider other)
    {
      isColliding = true;
      frozenY = _transform.position.y;
      //check if only terrain?
      //_rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
      Debug.Log("Collided with " + other.name);
    }

    public void OnTriggerExt(Collider other)
    {
      isColliding = false;
      _rigidbody.constraints = RigidbodyConstraints.None;
    }

    private void Translate()
    {
      if (Input.GetKey(KeyCode.W))
      {
        _transform.position += speed * Time.deltaTime * _transform.forward;
      }

      if (Input.GetKey(KeyCode.S))
      {
        _transform.position -= speed * Time.deltaTime * _transform.forward;
      }

      if (Input.GetKey(KeyCode.D))
      {
        _transform.position += speed * Time.deltaTime * _transform.right;
      }

      if (Input.GetKey(KeyCode.A))
      {
        _transform.position -= speed * Time.deltaTime * _transform.right;
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