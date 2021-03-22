using System;
using Ecosystem.Util;
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
    private float _boostFactor = 2;
    private bool _boosted = false;
    private bool _lookLocked = false;

    private void Start()
    {
      _rigidbody = GetComponent<Rigidbody>();
      _transform = _rigidbody.transform;
    }

    private void Update()
    {
      Boost();
      SelectTracked();
      TrackingOrWASD();
      Rotate();

      if (Input.GetKeyUp(KeyCode.Escape))
      {
        Application.Quit();
      }

      _rigidbody.velocity = _rigidbody.velocity.normalized * speed;
    }

    private Vector3 adjustmentVector3 = new Vector3(0, 0, 0);

    private void TrackingOrWASD()
    {
      if (_track)
      {
        if (_trackedTarget != null)
        {
          _distance += Input.mouseScrollDelta.y;
          _distance = Math.Max(_distance, 5);
          if (_lookLocked)
          {
            adjustmentVector3 = _trackedTarget.forward;
            _transform.LookAt(_trackedTarget);
          }
          else
          {
            adjustmentVector3 = Vector3.zero;
          }

          _transform.position = _trackedTarget.position + Vector3.up * _distance - adjustmentVector3 * _distance;
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
          if (Tags.IsAnimal(hit.transform.gameObject))
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
        speed *= _boostFactor;
      }

      if (Input.GetKeyUp(KeyCode.LeftShift) && _boosted)
      {
        _boosted = false;
        speed /= _boostFactor;
      }
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

      if (Input.GetKey(KeyCode.R))
      {
        _rigidbody.velocity += speed * Time.deltaTime * Vector3.up;
      }

      if (Input.GetKey(KeyCode.F))
      {
        _rigidbody.velocity += speed * Time.deltaTime * Vector3.down;
      }

      //Key up
      if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) ||
          Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.R) || Input.GetKeyUp(KeyCode.F))
      {
        _rigidbody.velocity = Vector3.zero;
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