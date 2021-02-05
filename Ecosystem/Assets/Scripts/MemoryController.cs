using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class MemoryController : MonoBehaviour
{
  private int capacity = 5;

  public enum Desire
  {
    Water,
    Food,
    Prey,
    Idle,
    Nothing
  }

  private List<(Desire, GameObject)> _memory;
  private int temp = 0;

  private void Start()
  {
    _memory = new List<(Desire, GameObject)>(capacity);
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

    _memory.Insert(temp, (res, other.gameObject));
    temp++;
    if (temp > 4) temp = 0;
  }

  //Get a list of all objects with the same Desire as asked for in res
  public List<GameObject> GetFromMemory(Enum res)
  {
    List<GameObject> list = new List<GameObject>();
    foreach ((Desire d, GameObject g) in _memory)
    {
      if (d.Equals(res))
      {
        list.Add(g);
      }
    }

    return list;
  }
}
