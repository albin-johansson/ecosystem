using UnityEngine;

namespace Ecosystem.UI
{
  public sealed class Billboard : MonoBehaviour
  {
    private Transform _cameraTransform;

    private void Start()
    {
      _cameraTransform = GameObject.FindWithTag("MainCamera").transform;
    }

    private void LateUpdate()
    {
      transform.LookAt(transform.position + _cameraTransform.forward);
    }
  }
}