using UnityEngine;

public sealed class CameraController : MonoBehaviour
{
  [SerializeField] private float speed = 8;

  private float _x;
  private float _y;
  private Vector3 _rotateValue;
  private Transform _transform;

  private void Start()
  {
    _transform = transform; // Cache reference for performance reasons
  }

  private void Update()
  {
    if (Input.GetKey(KeyCode.Mouse1))
    {
      Rotate();
      Translate();
    }
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