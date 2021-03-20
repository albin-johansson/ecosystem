using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.UI
{
  public sealed class ResourceBar : MonoBehaviour
  {
    [SerializeField] private Slider slider;

    public void SetMaxValue(float value)
    {
      slider.maxValue = value;
    }

    public void SetValue(float value)
    {
      slider.value = slider.maxValue - value;
    }
    
    public void SetSaturationValue(float value)
    {
      slider.value = Mathf.Min(slider.maxValue, value);
    }
  }
}