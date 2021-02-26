using UnityEngine;

namespace Ecosystem
{
  public class CameraShader : MonoBehaviour
  {
    [SerializeField] private Camera camera;
    [SerializeField] private Shader shader;
    Camera TempCam;
    Material _outlineMaterial;


    public void Start()
    {
      TempCam = new GameObject().AddComponent<Camera>();
      _outlineMaterial = new Material(Shader.Find("Standard"));
      //camera.RenderWithShader(shader, "");
      //Debug.Log("After RenderWithShader");
      //Camera.current.RenderWithShader(Shader.Find("Toon"), "");
    }


    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
      TempCam.CopyFrom(Camera.current);
      TempCam.backgroundColor = Color.black;
      TempCam.clearFlags = CameraClearFlags.Color;

      TempCam.cullingMask = 1 << LayerMask.NameToLayer("Outline");

      var rt = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.R8);
      TempCam.targetTexture = rt;

      TempCam.RenderWithShader(Shader.Find("Standard"), "");

      _outlineMaterial.SetTexture("_SceneTex", source);
      Graphics.Blit(rt, destination, _outlineMaterial);

      RenderTexture.ReleaseTemporary(rt);

      //camera.RenderWithShader(shader, "");
    }
  }
}