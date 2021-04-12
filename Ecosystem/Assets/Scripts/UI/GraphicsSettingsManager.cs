using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AntiAliasing = UnityEngine.Rendering.PostProcessing.PostProcessLayer.Antialiasing;

namespace Ecosystem.UI
{
  public sealed class GraphicsSettingsManager : MonoBehaviour
  {
    public delegate void GraphicsSettingsChanged(GraphicsSettings settings);

    public static event GraphicsSettingsChanged OnGraphicsSettingsChanged;

    [SerializeField] private Toggle ambientOcclusionToggle;
    [SerializeField] private Slider ambientOcclusionSlider;
    [SerializeField] private TMP_Text ambientOcclusionIntensityText;
    [SerializeField] private TMP_Dropdown antiAliasingDropdown;

    private static AntiAliasing _antiAliasingMode = AntiAliasing.SubpixelMorphologicalAntialiasing;
    private static bool _ambientOcclusionEnabled = true;
    private static float _ambientOcclusionIntensity = 0.25f;

    public void OnAntiAliasingChanged()
    {
      var item = antiAliasingDropdown.options[antiAliasingDropdown.value];

      _antiAliasingMode = item.text switch
      {
        "SMAA" => AntiAliasing.SubpixelMorphologicalAntialiasing,
        "TAA" => AntiAliasing.TemporalAntialiasing,
        "FXAA" => AntiAliasing.FastApproximateAntialiasing,
        "None" => AntiAliasing.None,
        _ => _antiAliasingMode
      };

      NotifyListeners();
    }

    public void OnAmbientOcclusionToggled()
    {
      _ambientOcclusionEnabled = ambientOcclusionToggle.isOn;
      ambientOcclusionSlider.gameObject.SetActive(_ambientOcclusionEnabled);
      ambientOcclusionIntensityText.gameObject.SetActive(_ambientOcclusionEnabled);

      NotifyListeners();
    }

    public void OnAmbientOcclusionValueChanged()
    {
      _ambientOcclusionIntensity = ambientOcclusionSlider.value;
      ambientOcclusionIntensityText.text = $"{(int) Mathf.Round(100f * _ambientOcclusionIntensity)}%";

      NotifyListeners();
    }

    public static GraphicsSettings CurrentSettings() => new GraphicsSettings
    {
      AntiAliasing = _antiAliasingMode,
      AmbientOcclusionEnabled = _ambientOcclusionEnabled,
      AmbientOcclusionIntensity = _ambientOcclusionIntensity
    };

    private static void NotifyListeners()
    {
      OnGraphicsSettingsChanged?.Invoke(CurrentSettings());
    }
  }
}