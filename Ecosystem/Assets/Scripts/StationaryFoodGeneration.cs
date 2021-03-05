using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ecosystem
{
  /// <summary>
  /// Will spawn a specified food on a selected spawner. Needs a gameObject with specific location were the food should be instantiated.
  /// Works well on the berry bush were there is loads of locations to spawn berries. 
  /// </summary>
  public class StationaryFoodGeneration : MonoBehaviour
  {
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject foodObject;
    [SerializeField] private float rate;

    private float _elapsedTime;
    private int _spawningLocations;
    private Transform _spawnLocation;
    private Transform _spawnLocationParent;

    //An extra child object was added to the original berry bush prefab to create some order to which berries in it´s hierarchy are locations and which are consumable. 
    private void Start()
    {
      _spawnLocationParent = spawner.transform.GetChild(0);
      _spawningLocations = _spawnLocationParent.childCount;
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
        _elapsedTime = 0;

        var location = Random.Range(0, _spawningLocations);

        _spawnLocation = _spawnLocationParent.GetChild(location).transform;

        Instantiate(foodObject, _spawnLocation.position, _spawnLocation.rotation, spawner.transform);
      }
    }
  }
}