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
    [SerializeField] private string keyToPool;

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
        if (nutritionalValue == 0)
        {
          ReturnToPool();
        }
      }
    }

    public double Consume(Double hunger)
    {
      if (hunger < nutritionalValue)
      {
        nutritionalValue -= hunger;
        return hunger;
      }

      OnFoodEaten?.Invoke(gameObject);
      ReturnToPool();

      return nutritionalValue;
    }

    public void SetNutrition(double value)
    {
      nutritionalValue = value;
    }
    
    public void SetKeyToPool(String key)
    {
      keyToPool = key;
    }

    private void ReturnToPool()
    {
      if (ObjectPoolHandler.instance.isPoolValid(keyToPool))
      {
        ObjectPoolHandler.instance.ReturnToPool(keyToPool, gameObject);
      }
      else
      {
        Destroy(gameObject);
      }
    }
  }
}