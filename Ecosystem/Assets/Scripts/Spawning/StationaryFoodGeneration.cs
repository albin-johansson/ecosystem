using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ecosystem.Spawning
{
  /// <summary>
  /// The handler script for the stationary food generators. Activates berries on the bush for visuals purposes.  
  /// </summary>
  public sealed class StationaryFoodGeneration : MonoBehaviour
  {
    [SerializeField] private Transform spawner;
    [SerializeField] private float rate;
    [SerializeField] private int numberOfBerries;

    private float _elapsedTime;
    private int _spawningLocations;

    public Stack<int> BerriesPlaced { get; } = new Stack<int>();
    public int AmountOfBerries { get; set; }

    //Deactivates all berries to make them invisible.  
    private void Start()
    {
      _spawningLocations = spawner.childCount - 1;

      for (var i = 0; i < _spawningLocations; ++i)
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
      numberOfBerries = AmountOfBerries;
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
          SpawnBerries();
        }
      }
    }

    private void SpawnBerries()
    {
      if (AmountOfBerries < _spawningLocations + 1)
      {
        var location = Random.Range(0, _spawningLocations);
        spawner.GetChild(location).transform.gameObject.SetActive(true);
        BerriesPlaced.Push(location);
        AmountOfBerries += 1;
      }
    }
  }
}