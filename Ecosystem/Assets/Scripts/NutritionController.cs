using System;
using System.Collections;
using System.Collections.Generic;
using Ecosystem.Spawning;
using UnityEngine;

namespace Ecosystem
{
  public class NutritionController : MonoBehaviour
  {
    [SerializeField] private float nutritionalValue;
    [SerializeField] private string keyToPool;

    public delegate void FoodEatenEvent(GameObject food);

    /// <summary>
    /// This event is emitted every time a food resource is consumed.
    /// </summary>
    public static event FoodEatenEvent OnFoodEaten;

    private void Update()
    {
      // Simulates nutritional decay
      if (nutritionalValue > 0)
      {
        nutritionalValue = Mathf.Max(0, nutritionalValue - Time.deltaTime * 0.1f);
      }
      else
      {
        ReturnToPool();
      }
    }

    public float Consume(float amount)
    {
      if (amount < nutritionalValue)
      {
        nutritionalValue -= amount;
        return amount;
      }
      else
      {
        OnFoodEaten?.Invoke(gameObject);
        ReturnToPool();
        return nutritionalValue;
      }
    }

    public void SetNutrition(float value)
    {
      nutritionalValue = value;
    }

    public void SetKeyToPool(string key)
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