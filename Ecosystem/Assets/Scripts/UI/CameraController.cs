using System;
using UnityEngine;

namespace Ecosystem.UI
{
  public sealed class CameraController : MonoBehaviour
  {
    [SerializeField] private float speed = 8;
    [SerializeField] private Rigidbody rigidBody;

    private const float BoostFactor = 2;

    private Camera _camera;
    private Vector3 _rotateValue;
    private Transform _transform;
    private Transform _trackedTarget;
    private Vector3 _adjustmentVector3 = Vector3.zero;
    private float _x;
    private float _y;
    private float _distance = 20;
    private bool _boosted;
    private bool _lookLocked;
    private bool _track;

    private void Start()
    {
      _camera = Camera.main;
      _transform = rigidBody.transform;
    }

    private void Update()
    {
      Boost();
      SelectTracked();
      TrackingOrMovement();
      Rotate();

      if (Input.GetKeyUp(KeyCode.Escape))
      {
        Application.Quit();
      }

      rigidBody.velocity = rigidBody.velocity.normalized * speed;
    }

    private void TrackingOrMovement()
    {
      if (_track)
      {
        if (_trackedTarget)
        {
          _distance += Input.mouseScrollDelta.y;
          _distance = Math.Max(_distance, 5);
          if (_lookLocked)
          {
            _adjustmentVector3 = _trackedTarget.forward;
            _transform.LookAt(_trackedTarget);
          }
          else
          {
            _adjustmentVector3 = Vector3.zero;
          }

          _transform.position = _trackedTarget.position + Vector3.up * _distance - _adjustmentVector3 * _distance;
        }
        else
        {
          _track = false;
          _lookLocked = false;
        }
      }
      else
      {
        Translate();
      }
    }

    private void SelectTracked()
    {
      if (Input.GetKey(KeyCode.Q))
      {
        _track = false;
        _lookLocked = false;
      }

      if (Input.GetKey(KeyCode.Mouse0))
      {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 100))
        {
          if (!hit.transform.tag.Equals("Terrain"))
          {
            _trackedTarget = hit.transform;
            _track = true;
            _lookLocked = true;
          }
        }
      }

      if (Input.GetKeyUp(KeyCode.F))
      {
        _lookLocked = !_lookLocked;
      }
    }

    private void Boost()
    {
      if (Input.GetKey(KeyCode.LeftShift) && !_boosted)
      {
        _boosted = true;
        speed *= BoostFactor;
      }

      if (Input.GetKeyUp(KeyCode.LeftShift) && _boosted)
      {
        _boosted = false;
        speed /= BoostFactor;
      }
    }

    private void Translate()
    {
      if (Input.GetKey(KeyCode.W))
      {
        rigidBody.velocity += speed * Time.deltaTime * _transform.forward;
      }

      if (Input.GetKey(KeyCode.S))
      {
        rigidBody.velocity -= speed * Time.deltaTime * _transform.forward;
      }

      if (Input.GetKey(KeyCode.D))
      {
        rigidBody.velocity += speed * Time.deltaTime * _transform.right;
      }

      if (Input.GetKey(KeyCode.A))
      {
        rigidBody.velocity -= speed * Time.deltaTime * _transform.right;
      }

      if (Input.GetKey(KeyCode.R))
      {
        rigidBody.velocity += speed * Time.deltaTime * Vector3.up;
      }

      if (Input.GetKey(KeyCode.F))
      {
        rigidBody.velocity += speed * Time.deltaTime * Vector3.down;
      }

      //Key up
      if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) ||
          Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.R) || Input.GetKeyUp(KeyCode.F))
      {
        rigidBody.velocity = Vector3.zero;
      }
    }

    private void Rotate()
    {
      if (!Input.GetKey(KeyCode.LeftControl) && !_lookLocked)
      {
        _y = Input.GetAxis("Mouse X") * 1.5f;
        _x = Input.GetAxis("Mouse Y") * 1.5f;
        _rotateValue = new Vector3(_x, _y * -1, 0);
        _transform.eulerAngles -= _rotateValue;
      }
    }
  }
}