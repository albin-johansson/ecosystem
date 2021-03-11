using System.Collections.Generic;
using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem
{
  public sealed class MemoryController : MonoBehaviour
  {
    private const int Capacity = 5;

    private List<(AnimalState, Vector3)> _memory;
    private int _nextMemoryLocation;

    private void Start()
    {
      _memory = new List<(AnimalState, Vector3)>(Capacity);
    }

    //Saves a game objects position and its desire to memory
    public void SaveToMemory(GameObject other)
    {
      _memory.Insert(_nextMemoryLocation, (GetAnimalState(other), other.gameObject.transform.position));
      ++_nextMemoryLocation;
      if (_nextMemoryLocation >= Capacity)
      {
        _nextMemoryLocation = 0;
      }
    }

    private static AnimalState GetAnimalState(GameObject other)
    {
      if (other.gameObject.layer == Layers.FoodLayer)
      {
        return AnimalState.LookingForFood;
      }
      else if (other.gameObject.layer == Layers.PreyLayer)
      {
        return AnimalState.LookingForPrey;
      }
      else if (other.gameObject.layer == Layers.WaterLayer)
      {
        return AnimalState.LookingForWater;
      }

      return AnimalState.Idle;
    }

    //Get a position of an object with the same Type as the Desire asked for in currentDesire
    public (bool, Vector3) GetFromMemory(AnimalState currentDesire)
    {
      int i = 0;
      foreach (var (desire, position) in _memory)
      {
        if (desire == currentDesire)
        {
          if (currentDesire != AnimalState.LookingForWater)
          {
            //Removes food resource from memory to prevent animal from wandering back to food that has already been consumed. 
            _memory.Insert(i, (AnimalState.Idle, new Vector3()));
          }

          return (true, position);
        }

        i++;
      }

      return (false, Vector3.zero);
    }
  }
}