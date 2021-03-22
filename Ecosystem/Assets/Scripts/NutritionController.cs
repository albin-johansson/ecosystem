using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem
{
    public class NutritionController : MonoBehaviour
    {

        [SerializeField] private float NutritionalValue;

        public float Consume(float hunger)
        {
            var consumptionAmount = Mathf.Min(hunger, NutritionalValue);
            NutritionalValue -= consumptionAmount;
            return consumptionAmount;
        }
    }
}
