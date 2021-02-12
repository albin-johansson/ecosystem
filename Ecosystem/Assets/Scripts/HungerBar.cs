using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetMaxHunger(float hunger)
    {
        slider.maxValue = hunger;
    }

    public void SetHunger(float hunger)
    {
        slider.value = slider.maxValue - hunger;
    }
}
