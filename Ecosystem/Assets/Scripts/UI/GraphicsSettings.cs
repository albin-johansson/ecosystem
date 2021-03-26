using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.UI
{
  public sealed class GraphicsSettings : MonoBehaviour
  {
    [SerializeField] private Sprite cogwheel;
    [SerializeField] private Sprite arrow;
    [SerializeField] private Image showSettingsButtonImage;
    [SerializeField] private GameObject mainMenuWindow;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject ambientOcclusionSlider;
    [SerializeField] private GameObject antialiasingDropdown;
    [SerializeField] private GameObject intensityText;
    [SerializeField] private GameObject intensityTextValue;
    
    public static int _antiAliasingMode;
    public static bool _ambientOcclusionActive = true;
    public static int _ambientOcclusionIntensity = 25;
    public static bool _update = false;

    public void SettingsMenu()
    {
      settingsWindow.SetActive(!settingsWindow.activeSelf);
      mainMenuWindow.SetActive(!mainMenuWindow.activeSelf);
      if (settingsWindow.activeSelf)
      {
        showSettingsButtonImage.sprite = arrow;
        _update = true;
      }
      else
      {
        showSettingsButtonImage.sprite = cogwheel;
        _update = false;
      }
    }

    public void AntiAliasingOption()
    {
      _antiAliasingMode = antialiasingDropdown.GetComponent<TMP_Dropdown>().value;
    }

    public void AmbientOcclusionActive()
    {
      _ambientOcclusionActive = !_ambientOcclusionActive;
      ambientOcclusionSlider.SetActive(!ambientOcclusionSlider.activeSelf);
      intensityText.SetActive(!intensityText.activeSelf);
      intensityTextValue.SetActive(!intensityTextValue.activeSelf);
    }

    public void AmbientOcclusionSlider()
    {
      _ambientOcclusionIntensity = (int) Math.Round(100 * ambientOcclusionSlider.GetComponent<Slider>().value);
      intensityTextValue.GetComponent<TMP_Text>().text = _ambientOcclusionIntensity + "%";
    }
  }
}