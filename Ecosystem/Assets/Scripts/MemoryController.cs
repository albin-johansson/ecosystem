using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem
{
  public sealed class MemoryController : MonoBehaviour
  {
    private const int MemoryCapacity = 5;

    private List<GameObject> _memory;
    private int _nextMemoryLocation;

    private void Start()
    {
      _memory = new List<GameObject>(MemoryCapacity);
    }

    public void SaveToMemory(GameObject other)
    {
      _memory.Insert(_nextMemoryLocation, (other.gameObject));
      ++_nextMemoryLocation;
      if (_nextMemoryLocation >= MemoryCapacity)
      {
        _nextMemoryLocation = 0;
      }
    }

    /// <summary>
    ///   Returns the GameObject from memory that is closest one to position,
    ///   from the objects that return true for the filter function.
    /// </summary>
    public GameObject GetClosestInMemory(Func<GameObject, bool> predicate, Vector3 position)
    {
      GameObject closest = null;
      _memory.RemoveAll(o => !o);
      foreach (var memoryObject in _memory)
      {
        if (!predicate(memoryObject))
        {
          continue;
        }
        if (!closest)
        {
          closest = memoryObject;
        }
        else if (Vector3.Distance(position, closest.transform.position) >
                 Vector3.Distance(position, memoryObject.transform.position))
        {
          closest = memoryObject;
        }
      }
      
      return closest;
    }
  }
}