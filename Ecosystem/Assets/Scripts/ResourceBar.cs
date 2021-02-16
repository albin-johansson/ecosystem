using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}