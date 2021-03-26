using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem
{
  public sealed class MemoryController : MonoBehaviour
  {
    private const int Capacity = 5;

    private List<GameObject> _memory;
    private int _nextMemoryLocation;

    private void Start()
    {
      _memory = new List<GameObject>(Capacity);
    }

    //Saves a game objects position and its desire to memory
    public void SaveToMemory(GameObject other)
    {
      _memory.Insert(_nextMemoryLocation, (other.gameObject));
      ++_nextMemoryLocation;
      if (_nextMemoryLocation >= Capacity)
      {
        _nextMemoryLocation = 0;
      }
    }

    public (bool, GameObject) GetFromMemory(string tag)
    {
      foreach (var memoryItem in _memory)
      {
        if (memoryItem.CompareTag(tag))
        {
          return (true, memoryItem);
        }
      }
      return (false, null);
    }
  }
}