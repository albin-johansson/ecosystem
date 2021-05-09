using System.Collections.Generic;
using Ecosystem.Consumer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ecosystem.Spawning
{
  /// <summary>
  /// The handler script for the stationary food generators. Activates berries on the bush for visuals purposes.  
  /// </summary>
  public sealed class StationaryFoodGeneration : MonoBehaviour
  {
    public delegate void GeneratedFood(GameObject food);

    public static event GeneratedFood OnGeneratedFood;

    public static event NutritionController.FoodEatenEvent OnFoodEaten;

    [SerializeField] private Transform spawner;
    [SerializeField] private float rate;

    private const int TotalBerryCount = 34;
    private float _elapsedTime;

    public int AmountOfBerries { get; private set; }

    /// Deactivates all berries to make them invisible.  
    private void Start()
    {
      for (var i = 0; i < spawner.childCount; ++i)
      {
        spawner.GetChild(i).gameObject.SetActive(false);
      }

      spawner.gameObject.SetActive(false);
    }

    /// <summary>
    /// The berry bush prefab has now a child with multiple invisible berries in it´s hierarchy.
    /// These invisible berries are set to visible to indicate how many berries exist on the bush. 
    /// </summary>
    private void Update()
    {
      _elapsedTime += Time.deltaTime;
      if (_elapsedTime > rate)
      {
        if (AmountOfBerries == 0)
        {
          spawner.gameObject.SetActive(true);
        }

        _elapsedTime = 0;
        for (var i = Random.Range(1, 4); i > 0; --i)
        {
          if (AmountOfBerries < spawner.childCount)
          {
            SpawnBerry();
          }
        }
      }
    }

    public void RemoveBerry()
    {
      if (AmountOfBerries != 0)
      {
        var index = AmountOfBerries - 1;

        var berryTransform = spawner.GetChild(index);
        var berryObject = berryTransform.gameObject;

        OnFoodEaten?.Invoke(berryObject);

        berryObject.SetActive(false);

        --AmountOfBerries;
      }
    }

    private void SpawnBerry()
    {
      Debug.Assert(AmountOfBerries < TotalBerryCount);

      var index = Random.Range(0, TotalBerryCount);

      var berryTransform = spawner.GetChild(index);
      var berryObject = berryTransform.gameObject;

      berryObject.SetActive(true);

      ++AmountOfBerries;

      OnGeneratedFood?.Invoke(berryObject);
    }
  }
}