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

    //Save gameobject and its "Desire" identifier as a tuple to _memory
    public void SaveToMemory(GameObject other)
    {
      var desire = GetDesire(other);
      _memory.Insert(_nextMemoryLocation, (desire, other.gameObject.transform.position));
      _nextMemoryLocation++;
      if (_nextMemoryLocation > (Capacity - 1))
      {
        _nextMemoryLocation = 0;
      }
    }

    private Desire GetDesire(GameObject other)
    {
      if (other.gameObject.layer.Equals(6))
      {
        return Desire.Food;
      }
      else if (other.gameObject.layer.Equals(8))
      {
        return Desire.Prey;
      }
      else if (other.gameObject.layer.Equals(4))
      {
        return Desire.Water;
      }

      return Desire.Idle;
    }

    public bool ExistInMemory(Desire currentDesire)
    {
      foreach (var (desire, position) in _memory)
      {
        if (desire.Equals(currentDesire))
        {
          return true;
        }
      }

      return false;
    }

    //Get a position of an object with the same Type as the Desire asked for in currentDesire
    public Vector3 GetFromMemory(Desire currentDesire)
    {
      foreach (var (desire, position) in _memory)
      {
        if (desire.Equals(currentDesire))
        {
          if (!currentDesire.Equals(Desire.Water))
          {
            _memory.Insert(_memory.IndexOf((desire, position)), (Desire.Nothing, new Vector3()));
          }

          return position;
        }
      }

      return Vector3.zero;
    }
  }
}