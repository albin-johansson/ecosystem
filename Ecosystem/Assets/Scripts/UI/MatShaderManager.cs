using UnityEngine;

namespace Ecosystem.UI
{
  public class MatShaderManager : MonoBehaviour
  {
    [SerializeField] private Material material;
    [SerializeField] private Shader shader;

    private void Start()
    {
      material.shader = shader;
    }
  }
}