using UnityEngine;

public sealed class CameraController : MonoBehaviour
{
  public float speed = 8;

  private float _x;
  private float _y;
  private Vector3 _rotateValue;

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
    var currentTransform = transform;
    if (Input.GetKey(KeyCode.W))
    {
      currentTransform.position += speed * Time.deltaTime * currentTransform.forward;
    }

    if (Input.GetKey(KeyCode.S))
    {
      currentTransform.position -= speed * Time.deltaTime * currentTransform.forward;
    }

    if (Input.GetKey(KeyCode.D))
    {
      currentTransform.position += speed * Time.deltaTime * currentTransform.right;
    }

    if (Input.GetKey(KeyCode.A))
    {
      currentTransform.position -= speed * Time.deltaTime * currentTransform.right;
    }
  }

  private void Rotate()
  {
    _y = Input.GetAxis("Mouse X");
    _x = Input.GetAxis("Mouse Y");
    _rotateValue = new Vector3(_x, _y * -1, 0);

    var currentTransform = transform;
    currentTransform.eulerAngles -= _rotateValue;
  }
}