using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Ecosystem
{
  public sealed class MemoryController : MonoBehaviour
  {
    private const int MemoryCapacity = 5;
    private const int InVisionCapacity = 10;

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

    public GameObject GetClosestInMemory(Func<GameObject, bool> filter, Vector3 position)
    {
      var r =  GetClosestFromList(filter, position, _memory);
      return r;
    }

    private static GameObject GetClosestFromList(Func<GameObject, bool> f, Vector3 p, List<GameObject> l)
    {
      GameObject closest = null;
      l.RemoveAll(GameObject => !GameObject);
      foreach (var r in l.Where(f))
      {
        if (!closest)
        {
          closest = r;
        }
        else if (Vector3.Distance(p, closest.transform.position) >
                 Vector3.Distance(p, r.transform.position))
        {
          closest = r;
        }
      }
      return closest;
    }
  }
}
