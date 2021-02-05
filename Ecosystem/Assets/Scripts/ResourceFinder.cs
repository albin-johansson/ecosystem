using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class ResourceFinder : MonoBehaviour
{
  [SerializeField] private FoodConsumer foodConsumer;
  [SerializeField] private WaterConsumer waterConsumer;

  [SerializeField] private MemoryController memoryController;
  [SerializeField] private TargetTracker targetTracker;

  private bool hasTarget = false;

  private MemoryController.Desire priority = MemoryController.Desire.Idle;

  private void Update()
  {
    SetPriority();
    CheckMemory();
  }

  public void SetHasTarget(bool state)
  {
    hasTarget = state;
  }

  //Checks MemoryController for objects that matches the priority Desire
  private void CheckMemory()
  {
    if (!hasTarget)
    {
      SetHasTarget(true);
      List<GameObject> temp = memoryController.GetFromMemory(priority);

      //Selects a random object that matches priority, if non exist set hasTarget to false again.
      if (temp.Count > 0)
      {
        GameObject g = temp[Random.Range(0, temp.Count - 1)];
        targetTracker.SetTarget(g);
      }
      else SetHasTarget(false);
    }
  }

  //Sets priority, needs to be worked on to get a better flow
  private void SetPriority()
  {
    // Hunger has implicit priority
    if (foodConsumer.IsHungry())
    {
      priority = MemoryController.Desire.Food;
    }
    else if (waterConsumer.IsThirsty())
    {
      priority = MemoryController.Desire.Water;
    }
    else priority = MemoryController.Desire.Idle;
  }

  //When colliding with an object that object is saved to MemoryController and then set as a target in TargetTracker if the priority matches.
  //Might be an improvment to only save the object and not set it as a target. 
  private void OnTriggerEnter(Collider other)
  {
    memoryController.SaveToMemory(other.gameObject);
    if (!hasTarget)
    {
      if (priority == MemoryController.Desire.Food && other.GetComponent<Food>() != null ||
          priority == MemoryController.Desire.Water && other.GetComponent<Water>() != null)
      {
        SetHasTarget(true);
        targetTracker.SetTarget(other.gameObject);
      }
    }
  }
}