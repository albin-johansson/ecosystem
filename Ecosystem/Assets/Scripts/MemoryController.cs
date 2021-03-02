using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem
{
  public sealed class MemoryController : MonoBehaviour
  {
    private const int Capacity = 5;

    private List<(Desire, Vector3)> _memory;
    private int _nextMemoryLocation;

    private void Start()
    {
      _memory = new List<(Desire, Vector3)>(Capacity);
    }

    //Saves a game objects position and its desire to memory
    public void SaveToMemory(GameObject other)
    {
      _memory.Insert(_nextMemoryLocation, (GetDesire(other), other.gameObject.transform.position));
      ++_nextMemoryLocation;
      if (_nextMemoryLocation >= Capacity)
      {
        _nextMemoryLocation = 0;
      }
    }

    private static Desire GetDesire(GameObject other)
    {
      if (other.gameObject.layer == LayerUtil.FoodLayer)
      {
        return Desire.Food;
      }
      else if (other.gameObject.layer == LayerUtil.PreyLayer)
      {
        return Desire.Prey;
      }
      else if (other.gameObject.layer == LayerUtil.WaterLayer)
      {
        return Desire.Water;
      }

      return Desire.Idle;
    }

    //Get a position of an object with the same Type as the Desire asked for in currentDesire
    public (bool, Vector3) GetFromMemory(Desire currentDesire)
    {
      int i = 0;
      foreach (var (desire, position) in _memory)
      {
        if (desire == currentDesire)
        {
          if (currentDesire != Desire.Water)
          {
            //Removes food resource from memory to prevent animal from wandering back to food that has already been consumed. 
            _memory.Insert(i, (Desire.Nothing, new Vector3()));
          }

          return (true, position);
        }

        i++;
      }

      return (false, Vector3.zero);
    }
  }
}