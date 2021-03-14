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
    private float _distance = 20;
    private Rigidbody _rigidbody;

    private void Start()
    {
      _rigidbody = GetComponent<Rigidbody>();
      _transform = _rigidbody.transform;
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
          }
        }
      }

      //Follow the tracked animal
      if (_track)
      {
        //Zoom with mouse wheel
        _distance += Input.mouseScrollDelta.y;
        _distance = Math.Max(_distance, 5);

        //follow
        _transform.position = _trackedTarget.position + Vector3.up * _distance;
        //TODO: could this be useful?
        //transform.LookAt(_trackedTarget);
      }
      //Move around with "WASD"
      else
      {
        Translate();
      }

      Rotate();

      //Exit application
      if (Input.GetKeyUp(KeyCode.Escape))
      {
        Application.Quit();
      }

      _rigidbody.velocity = _rigidbody.velocity.normalized * speed;
    }

    private void Translate()
    {
      if (Input.GetKey(KeyCode.W))
      {
        _rigidbody.velocity += speed * Time.deltaTime * _transform.forward;
      }

      if (Input.GetKey(KeyCode.S))
      {
        _rigidbody.velocity -= speed * Time.deltaTime * _transform.forward;
      }

      if (Input.GetKey(KeyCode.D))
      {
        _rigidbody.velocity += speed * Time.deltaTime * _transform.right;
      }

      if (Input.GetKey(KeyCode.A))
      {
        _rigidbody.velocity -= speed * Time.deltaTime * _transform.right;
      }

      //Key up
      if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) ||
          Input.GetKeyUp(KeyCode.A))
      {
        _rigidbody.velocity = Vector3.zero;
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