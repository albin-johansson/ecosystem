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
    var desire = GetDesire(other);
    _memory.Insert(_nextMemoryLocation, (desire, other.gameObject));
    _nextMemoryLocation++;
    if (_nextMemoryLocation > (Capacity - 1))
    {
      _nextMemoryLocation = 0;
    }
  }

  private Desire GetDesire(GameObject other)
  {
    var desire = Desire.Idle;
    if (other.GetComponent<Food>())
    {
      desire = Desire.Food;
    }
    else if (other.GetComponent<Prey>())
    {
      desire = Desire.Prey;
    }
    else if (other.GetComponent<Water>())
    {
      desire = Desire.Water;
    }

    return desire;
  }

  //Get a list of all objects with the same Desire as asked for in res
  public List<GameObject> GetFromMemory(Desire currentDesire)
  {
    List<GameObject> gameObjects = new List<GameObject>();
    foreach (var (desire, memoryGameObject) in _memory)
    {
      if (desire.Equals(currentDesire))
      {
        gameObjects.Add(memoryGameObject);
      }
    }

    return gameObjects;
  }
}