using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirstBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetMaxThirst(float thirst)
    {
        slider.maxValue = thirst;
    }

    public void SetThirst(float thirst)
    {
        slider.value = slider.maxValue - thirst;
    }
}
