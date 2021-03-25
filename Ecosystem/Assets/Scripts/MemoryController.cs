using System;
using System.Collections.Generic;
using System.Linq;
using Ecosystem.Util;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
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

    public void RemoveFromInVision(GameObject other)
    {
      _inVision.Remove(other);
    }
    
    public GameObject GetClosestInVision(Func<GameObject, bool> filter, Vector3 position)
    {
      GameObject closest = null;
      foreach (var obj in _inVision.Where(filter))
      {
        if (closest)
        {
          if (Vector3.Distance(position, closest.transform.position) >
              Vector3.Distance(position, obj.transform.position))
          {
            closest = obj;
          }
        }
        else
        {
          closest = obj;
        }
      }
      return closest;
    }

    public (bool, GameObject) GetFromInVision(string tag)
    {
      for (int i = 0; i < _inVision.Count; i++)
      {
        if (_inVision[i].CompareTag(tag))
        {
          return (true, _inVision[i]);
        }
      }

      return (false, null);
    }

    public (bool, GameObject) GetFromMemory(string tag)
    {
      for (int i = 0; i < _memory.Count; i++)
      {
        if (_memory[i].CompareTag(tag))
        {
          return (true, _memory[i]);
        }
      }

      return (false, null);
    }
  }
}