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
    private List<GameObject> _inVision;

    private bool _inVisionLock;

    private void Start()
    {
      _memory = new List<GameObject>(MemoryCapacity);
      _inVision = new List<GameObject>(InVisionCapacity);
    }

    //Saves a game objects position and its desire to memory
    public void SaveToMemory(GameObject other)
    {
      _memory.Insert(_nextMemoryLocation, (other.gameObject));
      ++_nextMemoryLocation;
      if (_nextMemoryLocation >= MemoryCapacity)
      {
        _nextMemoryLocation = 0;
      }
    }

    public void SaveToInVision(GameObject other)
    {
      _inVision.Add(other);
    }

    public IEnumerator RemoveFromInVision(GameObject other)
    {
      yield return new WaitUntil(() => !_inVisionLock);
      _inVision.Remove(other);
    }

    public GameObject GetClosestInVision(Func<GameObject, bool> filter, Vector3 position)
    {
      _inVisionLock = true;
      var r =  GetClosestFromList(filter, position, _inVision);
      _inVisionLock = false;
      return r;
    }

    public GameObject GetClosest(Func<GameObject, bool> filter, Vector3 position)
    {
      var closest = GetClosestInVision(filter, position);
      if (closest)
      {
        return closest;
      }
      return GetClosestFromList(filter, position, _memory);
    }

    private static GameObject GetClosestFromList(Func<GameObject, bool> f, Vector3 p, List<GameObject> l)
    {
      GameObject closest = null;
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