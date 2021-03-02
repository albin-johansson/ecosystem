using UnityEngine;

namespace Ecosystem
{
  public sealed class PreyFinder : MonoBehaviour
  {
    [SerializeField] private PreyConsumer preyConsumer;
    [SerializeField] private WaterConsumer waterConsumer;
    [SerializeField] private MemoryController memoryController;
    [SerializeField] private TargetTracker targetTracker;

    private AnimalState _state = AnimalState.Idle;

    private void Update()
    {
      UpdatePriority();
      CheckMemory();
    }

    //Checks the memory for objects that matches the prioritised desire
    private void CheckMemory()
    {
      if (!targetTracker.HasTarget && _state != AnimalState.Idle)
      {
        var (match, vector3) = memoryController.GetFromMemory(_state);
        if (match)
        {
          targetTracker.SetTarget(vector3, _state);
        }
      }
    }

    //Sets priority, will set priority of what is currently most needed.
    private void UpdatePriority()
    {
      if (waterConsumer.IsDrinking)
      {
        _state = AnimalState.Drinking;
        targetTracker.StopTracking();
        return;
      }
      else
      {
        targetTracker.ResumeTracking();
      }

      if (preyConsumer.Hunger > waterConsumer.Thirst && preyConsumer.IsHungry())
      {
        _state = AnimalState.LookingForPrey;
      }
      else if (waterConsumer.Thirst > preyConsumer.Hunger && waterConsumer.IsThirsty())
      {
        _state = AnimalState.LookingForWater;
      }
      else
      {
        _state = AnimalState.Idle;
      }
    }

    /// <summary>
    /// When colliding with an object, that object is saved to the animals memory, and subsequently set as a target if the
    /// priority matches.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
      memoryController.SaveToMemory(other.gameObject);

      if (!targetTracker.HasTarget)
      {
        if (_state == AnimalState.LookingForPrey && other.gameObject.layer == LayerUtil.PreyLayer ||
            _state == AnimalState.LookingForWater && other.gameObject.layer == LayerUtil.WaterLayer)
        {
          targetTracker.SetTarget(other.gameObject.transform.position, _state);
        }
      }
    }
  }
}
