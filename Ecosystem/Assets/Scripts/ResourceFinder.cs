using System;
using Ecosystem.Genes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ecosystem
{
  public sealed class ResourceFinder : MonoBehaviour
  {
    [SerializeField] private FoodConsumer foodConsumer;
    [SerializeField] private WaterConsumer waterConsumer;

    [SerializeField] private MemoryController memoryController;
    [SerializeField] private TargetTracker targetTracker;

    private Desire _priority = Desire.Idle;

    const int WaterLayer = 4;
    const int FoodLayer = 6;
    const int WolfLayer = 7;

    private void Update()
    {
      UpdatePriority();
      CheckMemory();
    }

    //Checks MemoryController for objects that matches the priority Desire
    private void CheckMemory()
    {
      if (!targetTracker.HasTarget)
      {
        if (memoryController.ExistInMemory(_priority))
        {
          targetTracker.SetTarget(memoryController.GetFromMemory(_priority), _priority);
        }
      }
    }

    //Sets priority, will set priority of what is currently most needed.
    private void UpdatePriority()
    {
      if (foodConsumer.MyHunger() > waterConsumer.MyThirst() && foodConsumer.IsHungry())
      {
        _priority = Desire.Food;
      }
      else if (waterConsumer.MyThirst() > foodConsumer.MyHunger() && waterConsumer.IsThirsty())
      {
        _priority = Desire.Water;
      }
      else
      {
        _priority = Desire.Idle;
      }
    }

    /// <summary>
    /// When colliding with an object that object is saved to MemoryController and then set as a target in TargetTracker if the priority matches.
    /// Might be an improvment to only save the object and not set it as a target.
    /// If a predator is found the targetTracker resolves the fleeing mechanics.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.layer.Equals(WolfLayer))
      {
        targetTracker.FleeFromPredator(other.gameObject);
        return;
      }

      memoryController.SaveToMemory(other.gameObject);

      if (!targetTracker.HasTarget)
      {
        if (_priority == Desire.Food && other.gameObject.layer.Equals(FoodLayer) ||
            _priority == Desire.Water && other.gameObject.layer.Equals(WaterLayer))
        {
          targetTracker.SetTarget(other.gameObject.transform.position, _priority);
        }
      }
    }
  }
}