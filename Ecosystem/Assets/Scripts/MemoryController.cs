using System.Collections.Generic;
using Ecosystem;
using UnityEngine;

public sealed class MemoryController : MonoBehaviour
{
  private const int Capacity = 5;

  private List<(Desire, GameObject)> _memory;
  private int _nextMemoryLocation;

  private void Start()
  {
    _memory = new List<(Desire, GameObject)>(Capacity);
  }

  //Save gameobject and its "Desire" identifier as a tuple to _memory
  public void SaveToMemory(GameObject other)
  {
    _memory.Insert(_nextMemoryLocation, (GetDesire(other), other.gameObject));
    ++_nextMemoryLocation;
    if (_nextMemoryLocation >= Capacity)
    {
      _nextMemoryLocation = 0;
    }
  }

  /// <summary>
  /// Looks for a game object in the memory with the specified desire.
  /// </summary>
  /// <param name="desire">The desire to look for the in the memory</param>
  /// <returns>The first game object associated with the specified desire; null if no such object exists.</returns>
  public GameObject RecallFromMemory(Desire desire)
  {
    foreach (var (memoryDesire, memoryGameObject) in _memory)
    {
      if (desire == memoryDesire && memoryGameObject)
      {
        return memoryGameObject;
      }
    }

    return null;
  }

  private static Desire GetDesire(GameObject other)
  {
    if (other.CompareTag("Food"))
    {
      return Desire.Food;
    }
    else if (other.CompareTag("Prey"))
    {
      return Desire.Prey;
    }
    else if (other.GetComponent<Water>())
    {
      return Desire.Water;
    }
    else
    {
      return Desire.Idle;
    }
  }
}