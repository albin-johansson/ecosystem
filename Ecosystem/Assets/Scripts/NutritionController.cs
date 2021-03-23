using System;
using System.Collections;
using System.Collections.Generic;
using Ecosystem.Spawning;
using UnityEngine;

namespace Ecosystem
{
    public class NutritionController : MonoBehaviour
    {

        [SerializeField] private Double NutritionalValue;

        public delegate void FoodEatenEvent(GameObject food);

        /// <summary>
        /// This event is emitted every time a food resource is consumed.
        /// </summary>
        public static event FoodEatenEvent OnFoodEaten;
        
        public double Consume(Double hunger)
        {
            if (hunger < NutritionalValue)
            {
                NutritionalValue -= hunger;
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
            
            return NutritionalValue;
        }
    }
}
