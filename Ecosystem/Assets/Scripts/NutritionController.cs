using System;
using System.Collections;
using System.Collections.Generic;
using Ecosystem.Spawning;
using UnityEngine;

namespace Ecosystem
{
    public class NutritionController : MonoBehaviour
    {
        public double nutritionalValue;

        public delegate void FoodEatenEvent(GameObject food);

        /// <summary>
        /// This event is emitted every time a food resource is consumed.
        /// </summary>
        public static event FoodEatenEvent OnFoodEaten;

        public void Update()
        {
            // Simulates nutritional decay.
            if (nutritionalValue > 0)
            {
                nutritionalValue = (double) Mathf.Max((float) 0, (float) nutritionalValue - Time.deltaTime * 0.1f);
            }
        }
        public double Consume(Double hunger)
        {
            Debug.Log("Consume: " + hunger);
            if (hunger < nutritionalValue)
            {
                nutritionalValue -= hunger;
                return hunger;
            }
            
            OnFoodEaten?.Invoke(gameObject);
            var gameObjectTag = gameObject.tag;
            if (ObjectPoolHandler.instance.isPoolValid(gameObjectTag))
            {
                ObjectPoolHandler.instance.ReturnToPool(gameObjectTag, gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            return nutritionalValue;
        }

        public void SetNutrition(double value)
        {
            nutritionalValue = value;
        }
    }
}