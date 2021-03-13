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
    private float distance = 20; //TODO: add support for changing distance, scroll wheel?

    private void Start()
    {
      _transform = transform; // Cache reference for performance reasons
    }

    private void Update()
    {
      if (Input.GetKey(KeyCode.Q))
      {
        _track = false;
      }

      if (Input.GetKey(KeyCode.Mouse0))
      {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
          //TODO: check if hit is animal. else it is terrain so ignore it. 
          _trackedTarget = hit.transform;
          _track = true;
          Debug.Log(hit.transform.gameObject.name);
        }
      }

      if (_track)
      {
        transform.position = _trackedTarget.position + Vector3.up * distance;
      }
      else
      {
        if (Input.GetKey(KeyCode.Mouse1))
        {
          Translate();
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
          Application.Quit();
        }
      }

      //TODO: how to best handle rotate(). always or sometimes?
      Rotate();
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