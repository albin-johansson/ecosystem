using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ecosystem
{
  /// <summary>
  /// The handler script for the stationary food generators. Actives children meshes for visuals.  
  /// </summary>
  public class StationaryFoodGeneration : MonoBehaviour
  {
    [SerializeField] private Transform spawner;
    [SerializeField] private float rate;

    private float _elapsedTime;
    private int _spawningLocations;
    private Transform _spawnerChild;

    public Stack<int> BerriesPlaced { get; private set; } = new Stack<int>();
    public int AmountOfBerries { get; set; }

    //An extra child object was added to the original berry bush prefab to create some order to which berries in it´s hierarchy are locations and which are consumable. 
    private void Start()
    {
      _spawningLocations = spawner.childCount - 1;

      for (int i = 0; i < _spawningLocations; i++)
      {
        spawner.GetChild(i).gameObject.SetActive(false);
      }

      spawner.gameObject.SetActive(false);
    }

    /// <summary>
    /// The berry bush prefab has now a child with multiple invisible berries in it´s hierarchy.
    /// These invisible berries act as spawning locations for actual berries which can be eaten by animals.
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
}