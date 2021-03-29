using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

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
    public GameObject GetClosestInMemory(Func<GameObject, bool> filter, Vector3 position)
    {
      GameObject closest = null;
      _memory.RemoveAll(o => !o);
      foreach (var o in _memory.Where(filter))
      {
        if (!closest)
        {
          closest = o;
        }
        else if (Vector3.Distance(position, closest.transform.position) >
                 Vector3.Distance(position, o.transform.position))
        {
          closest = o;
        }
      }
      return closest;
    }
  }
}