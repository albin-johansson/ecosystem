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

  // Checks the memory for objects that matches the prioritised desire
  private void CheckMemory()
  {
    if (!targetTracker.HasTarget)
    {
      var target = memoryController.RecallFromMemory(_priority);
      if (target)
      {
        targetTracker.SetTarget(target);
      }
    }
  }

  // TODO needs to be worked on to get a better flow
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
  /// When colliding with an object, that object is saved to the animals memory, and subsequently set as a target if the
  /// priority matches.
  /// </summary>
  /// TODO Might be an improvment to only save the object and not set it as a target.
  private void OnTriggerEnter(Collider other)
  {
    memoryController.SaveToMemory(other.gameObject);
    if (!targetTracker.HasTarget)
    {
      if (_priority == Desire.Prey && other.GetComponent<Prey>() != null ||
          _priority == Desire.Water && other.GetComponent<Water>() != null)
      {
        targetTracker.SetTarget(other.gameObject);
      }
    }
  }
}