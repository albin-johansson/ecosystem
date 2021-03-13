using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Ecosystem.UI
{
  public class HandlePostProcessing : MonoBehaviour
  {
    [SerializeField] private PostProcessVolume ppVolume;
    [SerializeField] private PostProcessLayer ppLayer;

    private void Start()
    {
      switch (GraphicsSettings._antiAliasingMode)
      {
        case 0:
          ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
          break;
        case 1:
          ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
          break;
        case 2:
          ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
          break;
        case 3:
          ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;
          break;
        default: break;
      }

      ppVolume.profile.GetSetting<AmbientOcclusion>().active = GraphicsSettings._ambientOcclusionActive;
      ppVolume.profile.GetSetting<AmbientOcclusion>().intensity.value =
        (float) GraphicsSettings._ambientOcclusionIntensity / 100;
    }
  }
}