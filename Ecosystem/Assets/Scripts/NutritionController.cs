using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem
{
    public class NutritionController : MonoBehaviour
    {

        [SerializeField] private Double NutritionalValue;

        public double Consume(Double hunger)
        {
            if (hunger < NutritionalValue)
            {
                NutritionalValue -= hunger;
                return hunger;
            }

            return NutritionalValue;
        }
    }
}
