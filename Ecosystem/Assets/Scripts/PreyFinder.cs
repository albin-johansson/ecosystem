using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ecosystem
{
  public sealed class PreyFinder : MonoBehaviour
  {
    [SerializeField] private PreyConsumer preyConsumer;
    [SerializeField] private WaterConsumer waterConsumer;

    [SerializeField] private MemoryController memoryController;
    [SerializeField] private TargetTracker targetTracker;

    private Desire _priority = Desire.Idle;

    const int WaterLayer = 4;
    const int RabbitLayer = 8;

    private void Update()
    {
      UpdatePriority();
      CheckMemory();
    }

    //Checks MemoryController for objects that matches the priority Desire
    private void CheckMemory()
    {
      if (!targetTracker.HasTarget && !_priority.Equals(Desire.Idle))
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
      if (preyConsumer.MyHunger() > waterConsumer.MyThirst() && preyConsumer.IsHungry())
      {
        _priority = Desire.Prey;
      }
      else if (waterConsumer.MyThirst() > preyConsumer.MyHunger() && waterConsumer.IsThirsty())
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
      memoryController.SaveToMemory(other.gameObject);

      if (!targetTracker.HasTarget)
      {
        if (_priority == Desire.Food && other.gameObject.layer.Equals(RabbitLayer) ||
            _priority == Desire.Water && other.gameObject.layer.Equals(WaterLayer))
        {
          targetTracker.SetTarget(other.gameObject.transform.position, _priority);
        }
      }
    }
  }
}