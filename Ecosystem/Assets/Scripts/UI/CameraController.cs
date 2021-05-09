using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.UI
{
  public sealed class CameraController : MonoBehaviour
  {
    [SerializeField] private float speed = 8;
    [SerializeField] private Rigidbody cameraRigidBody;

    private const KeyCode QuitKey = KeyCode.Escape;
    private const KeyCode QuitTrackKey = KeyCode.Q;
    private const KeyCode BoostKey = KeyCode.LeftShift;
    private const KeyCode FreeLookKey = KeyCode.Mouse1;
    private const KeyCode ForwardKey = KeyCode.W;
    private const KeyCode LeftKey = KeyCode.A;
    private const KeyCode BackwardKey = KeyCode.S;
    private const KeyCode RightKey = KeyCode.D;
    private const KeyCode AscendKey = KeyCode.R;
    private const KeyCode DescendKey = KeyCode.F;
    private const float BoostFactor = 2;

    private Vector3 _rotateValue;
    private Transform _transform;
    private Transform _trackedTarget;
    private Camera _camera;
    private float _x;
    private float _y;
    private float _distance = 20;
    private bool _tracking;
    private bool _boosted;

    private void Start()
    {
      _camera = Camera.main;
      _transform = cameraRigidBody.transform;
      Cursor.visible = false;
    }

    private void Update()
    {
      UpdateBoost();
      UpdateTrackingState();

      if (_tracking)
      {
        TrackAnimal();
      }
      else
      {
        UpdateVelocity();
      }

      UpdateRotation();

      if (Input.GetKeyUp(QuitKey))
      {
        Application.Quit();
      }

      cameraRigidBody.velocity = cameraRigidBody.velocity.normalized * speed;
    }

    private void UpdateBoost()
    {
      if (Input.GetKey(BoostKey) && !_boosted)
      {
        _boosted = true;
        speed *= BoostFactor;
      }

      if (Input.GetKeyUp(BoostKey) && _boosted)
      {
        _boosted = false;
        speed /= BoostFactor;
      }
    }

    private void UpdateTrackingState()
    {
      if (Input.GetKey(QuitTrackKey))
      {
        _tracking = false;
      }
      else if (Input.GetKey(KeyCode.Mouse0))
      {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 100, Layers.AnimalMask))
        {
          _trackedTarget = hit.transform;
          _tracking = true;
        }
      }
    }

    private void TrackAnimal()
    {
      if (_trackedTarget)
      {
        _distance -= Input.mouseScrollDelta.y;
        _distance = Mathf.Max(_distance, 5);

        _transform.LookAt(_trackedTarget);
        var target = _trackedTarget.position + Vector3.up * _distance - _trackedTarget.forward * _distance;
        _transform.position = Vector3.Lerp(_transform.position, target, 0.1f);
      }
      else
      {
        _tracking = false;
      }
    }

    private void UpdateVelocity()
    {
      if (Input.GetKey(ForwardKey))
      {
        cameraRigidBody.velocity += speed * Time.deltaTime * _transform.forward;
      }

      if (Input.GetKey(BackwardKey))
      {
        cameraRigidBody.velocity -= speed * Time.deltaTime * _transform.forward;
      }

      if (Input.GetKey(RightKey))
      {
        cameraRigidBody.velocity += speed * Time.deltaTime * _transform.right;
      }

      if (Input.GetKey(LeftKey))
      {
        cameraRigidBody.velocity -= speed * Time.deltaTime * _transform.right;
      }

      if (Input.GetKey(AscendKey))
      {
        cameraRigidBody.velocity += speed * Time.deltaTime * Vector3.up;
      }

      if (Input.GetKey(DescendKey))
      {
        cameraRigidBody.velocity += speed * Time.deltaTime * Vector3.down;
      }

      if (Input.GetKeyUp(ForwardKey) ||
          Input.GetKeyUp(BackwardKey) ||
          Input.GetKeyUp(RightKey) ||
          Input.GetKeyUp(LeftKey) ||
          Input.GetKeyUp(AscendKey) ||
          Input.GetKeyUp(DescendKey))
      {
        cameraRigidBody.velocity = Vector3.zero;
      }
    }

    private void UpdateRotation()
    {
      if (!_tracking && Input.GetKey(FreeLookKey))
      {
        _y = Input.GetAxis("Mouse X") * 1.5f;
        _x = Input.GetAxis("Mouse Y") * 1.5f;
        _rotateValue = new Vector3(_x, _y * -1, 0);
        _transform.eulerAngles -= _rotateValue;
      }
    }
  }
}