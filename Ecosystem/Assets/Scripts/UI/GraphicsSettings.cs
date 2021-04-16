using AntiAliasing = UnityEngine.Rendering.PostProcessing.PostProcessLayer.Antialiasing;

namespace Ecosystem.UI
{
  public struct GraphicsSettings
  {
    public AntiAliasing AntiAliasing;
    public bool AmbientOcclusionEnabled;
    public float AmbientOcclusionIntensity;
  }
}