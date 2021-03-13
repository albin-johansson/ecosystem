using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.UI
{
  public sealed class GraphicsSettings : MonoBehaviour
  {
    [SerializeField] private GameObject settingsWindow;

    public static int _antiAliasingMode;
    public static bool _ambientOcclusionActive = true;
    public static int _ambientOcclusionIntensity = 25;

    public void SettingsMenu()
    {
      settingsWindow.SetActive(!settingsWindow.activeSelf);
    }

    public void AntiAliasingOption()
    {
      _antiAliasingMode = gameObject.GetComponent<Dropdown>().value;
    }

    public void AmbientOcclusionActive()
    {
      _ambientOcclusionActive = !_ambientOcclusionActive;
    }

    public void AmbientOcclusionSlider()
    {
      _ambientOcclusionIntensity = (int) Math.Round(100 * gameObject.GetComponent<Slider>().value);
    }
  }
}