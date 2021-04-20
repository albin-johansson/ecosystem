using Ecosystem.Spawning;
using UnityEngine;

namespace Ecosystem
{
  public sealed class NutritionController : MonoBehaviour
  {
    [SerializeField] private float nutritionalValue;

    public delegate void FoodEatenEvent(GameObject food);

    /// <summary>
    /// This event is emitted every time a food resource is consumed.
    /// </summary>
    public static event FoodEatenEvent OnFoodEaten;

    private string _keyToPool;

    private void Start()
    {
      _keyToPool = gameObject.tag;
    }

    private void Update()
    {
      // Simulates nutritional decay
      if (nutritionalValue > 0)
      {
        nutritionalValue = Mathf.Max(0, nutritionalValue - Time.deltaTime * 0.1f);
      }
      else
      {
        ObjectPoolHandler.Instance.ReturnOrDestroy(_keyToPool, gameObject);
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
        if (_keyToPool == null)
        {
          Start();
        }
        ObjectPoolHandler.Instance.ReturnOrDestroy(_keyToPool, gameObject);
        return nutritionalValue;
      }
    }

    public void SetNutritionalValue(float value)
    {
      nutritionalValue = value;
    }
  }
}