using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Ecosystem.UI
{
  public sealed class PostProcessingListener : MonoBehaviour
  {
    [SerializeField] private PostProcessVolume volume;
    [SerializeField] private PostProcessLayer layer;

    private void Start()
    {
      GraphicsSettingsManager.OnGraphicsSettingsChanged += OnGraphicsSettingsChanged;
      OnGraphicsSettingsChanged(GraphicsSettingsManager.CurrentSettings());
    }

    private void OnGraphicsSettingsChanged(GraphicsSettings settings)
    {
      layer.antialiasingMode = settings.AntiAliasing;

      var ambientOcclusion = volume.profile.GetSetting<AmbientOcclusion>();
      ambientOcclusion.active = settings.AmbientOcclusionEnabled;
      ambientOcclusion.intensity.value = settings.AmbientOcclusionIntensity;
    }
  }
}