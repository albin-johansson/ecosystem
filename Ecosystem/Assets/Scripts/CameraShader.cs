using UnityEngine;

namespace Ecosystem
{
  public class CameraShader : MonoBehaviour
  {
    [SerializeField] private Camera camera;
    [SerializeField] private Shader shader;

    public void Start()
    {
      camera.RenderWithShader(shader, "Opaque");
      Debug.Log("After RenderWithShader");
      //Camera.current.RenderWithShader(Shader.Find("Toon"), "");
    }


    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
      //camera.RenderWithShader(shader, "");
    }
  }
}