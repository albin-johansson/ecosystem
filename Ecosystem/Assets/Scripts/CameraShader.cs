using UnityEngine;

namespace Ecosystem
{
  public class CameraShader : MonoBehaviour
  {
    [SerializeField] private Camera camera;
    [SerializeField] private Shader shader;

    public void Start()
    {
      camera.RenderWithShader(shader, "");
      Debug.Log("After RenderWithShader");
      //Camera.current.RenderWithShader(Shader.Find("Toon"), "");
    }
  }
}