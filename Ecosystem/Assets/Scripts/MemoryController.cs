using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class MemoryController : MonoBehaviour
{
  private const int Capacity = 5;

  public enum Desire
  {
    Water,
    Food,
    Prey,
    Idle,
    Nothing
  }

  private List<(Desire, GameObject)> _memory;
  private int _nextMemoryLocation;

  private void Start()
  {
    _memory = new List<(Desire, GameObject)>(Capacity);
  }

  //Save gameobject and its "Desire" identifier as a tuple to _memory
  public void SaveToMemory(GameObject other)
  {
    Desire res = Desire.Idle;
    if (other.GetComponent<Food>())
    {
      res = Desire.Food;
    }
    else if (other.GetComponent<Prey>())
    {
      res = Desire.Prey;
    }
    else if (other.GetComponent<Water>())
    {
      res = Desire.Water;
    }

    _memory.Insert(_nextMemoryLocation, (res, other.gameObject));
    _nextMemoryLocation++;
    if (_nextMemoryLocation > 4) _nextMemoryLocation = 0;
  }

  //Get a list of all objects with the same Desire as asked for in res
  public List<GameObject> GetFromMemory(Enum res)
  {
    List<GameObject> list = new List<GameObject>();
    foreach (var (desire, memoryGameObject) in _memory)
    {
      if (desire.Equals(res))
      {
        list.Add(memoryGameObject);
      }
    }

    return list;
  }
}
