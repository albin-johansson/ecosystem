using System.Collections.Generic;
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
    if (other.GetComponent<Food>())
    {
      return Desire.Food;
    }
    else if (other.GetComponent<Prey>())
    {
      return Desire.Prey;
    }
    else if (other.GetComponent<Water>())
    {
      return Desire.Water;
    }

    return Desire.Idle;
  }

  //Get a list of all objects with the same Desire as asked for in currentDesire
  public List<GameObject> GetFromMemory(Desire currentDesire)
  {
    List<GameObject> gameObjects = new List<GameObject>();
    List<int> removeObjects = new List<int>();
    foreach (var (desire, memoryGameObject) in _memory)
    {
      if (desire.Equals(currentDesire))
      {
        if (memoryGameObject.Equals(null))
        {
          removeObjects.Add(_memory.IndexOf((desire, memoryGameObject)));
        }
        else
        {
          gameObjects.Add(memoryGameObject);
        }
      }
    }
    //Removes all objects that has been destroyed but still existed in memory with the same desire as currentDesire
    foreach (var i in removeObjects)
    {
      _memory.Insert(i, (Desire.Nothing, gameObject));
    }

    return gameObjects;
  }
}