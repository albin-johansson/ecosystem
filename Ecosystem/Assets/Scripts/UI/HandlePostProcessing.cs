using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Ecosystem.UI
{
  public sealed class HandlePostProcessing : MonoBehaviour
  {
    [SerializeField] private PostProcessVolume ppVolume;
    [SerializeField] private PostProcessLayer ppLayer;
    private bool _starting = true;
    
    private const int MSAA = 0;
    private const int TAA = 1;
    private const int FXAA = 2;
    private const int None = 3;
    
    private void Start()
    {
      Update();
    }

    private void Update()
    {
      if (_starting || GraphicsSettings._update)
      {
        _starting = false;
        switch (GraphicsSettings._antiAliasingMode)
        {
          case MSAA:
            ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
            break;
          case TAA:
            ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
            break;
          case FXAA:
            ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
            break;
          case None:
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
}