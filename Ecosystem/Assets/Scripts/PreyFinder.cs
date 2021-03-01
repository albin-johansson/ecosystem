using UnityEngine;

namespace Ecosystem
{
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
      if (waterConsumer.IsDrinking)
      {
        _priority = Desire.Drink;
        Debug.Log("Wolf is drinking");
        targetTracker.StopTracking();
        return;
      } 
      else
      {
        targetTracker.ResumeTracking();
      }

      
      if (waterConsumer.IsThirsty())
      {
        _priority = Desire.Water;
      }
      else if (preyConsumer.IsHungry())
      {
        _priority = Desire.Prey;
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
    /// TODO Might be an improvement to only save the object and not set it as a target.
    private void OnTriggerEnter(Collider other)
    {
      if(other.CompareTag("Water")){Debug.Log("Wolf see water");}
      memoryController.SaveToMemory(other.gameObject);
      if (!targetTracker.HasTarget)
      {
        if (_priority == Desire.Prey && other.CompareTag("Prey") ||
            _priority == Desire.Water && other.CompareTag("Water"))
        {
          targetTracker.SetTarget(other.gameObject);
        }
      }
    }
  }
}