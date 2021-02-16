using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class PreyFinder : MonoBehaviour
{
  [SerializeField] private PreyConsumer preyConsumer;
  [SerializeField] private WaterConsumer waterConsumer;

  [SerializeField] private MemoryController memoryController;
  [SerializeField] private TargetTracker targetTracker;

  private Desire _priority = Desire.Idle;

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

  //Sets priority, needs to be worked on to get a better flow
  private void UpdatePriority()
  {
    // Hunger has implicit priority
    if (preyConsumer.IsHungry())
    {
      _priority = Desire.Prey;
    }
    else if (waterConsumer.IsThirsty())
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
      if (_priority == Desire.Food && other.gameObject.layer.Equals(8) ||
          _priority == Desire.Water && other.gameObject.layer.Equals(4))
      {
        targetTracker.SetTarget(other.gameObject.transform.position, _priority);
      }
    }
  }
}