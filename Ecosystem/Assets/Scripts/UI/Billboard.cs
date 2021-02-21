using UnityEngine;

namespace Ecosystem
{
  // TODO move this to UI namespace
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