using UnityEngine;

namespace Ecosystem
{
  public sealed class ResourceFinder : MonoBehaviour
  {
    [SerializeField] private FoodConsumer foodConsumer;
    [SerializeField] private WaterConsumer waterConsumer;
    [SerializeField] private MemoryController memoryController;
    [SerializeField] private TargetTracker targetTracker;

    private AnimalState _state = AnimalState.Idle;

    private void Update()
    {
      UpdateState();
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
    private void UpdateState()
    {
      if (targetTracker.IsChased)
      {
        _state = AnimalState.Fleeing;
        waterConsumer.StopDrinking();
        targetTracker.ResumeTracking();
        return;
      }

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

      if (foodConsumer.Hunger > waterConsumer.Thirst && foodConsumer.IsHungry())
      {
        _state = AnimalState.LookingForFood;
      }
      else if (waterConsumer.Thirst > foodConsumer.Hunger && waterConsumer.IsThirsty())
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
    /// If a predator is within field of view the animal will flee.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
      if (LayerUtil.IsPredatorLayer(other.gameObject.layer))
      {
        _state = AnimalState.Fleeing;
        targetTracker.FleeFromPredator(other.gameObject);
        return;
      }

      memoryController.SaveToMemory(other.gameObject);

      if (!targetTracker.HasTarget)
      {
        if (_state == AnimalState.LookingForFood && other.gameObject.layer == LayerUtil.FoodLayer ||
            _state == AnimalState.LookingForWater && other.gameObject.layer == LayerUtil.WaterLayer)
        {
          targetTracker.SetTarget(other.gameObject.transform.position, _state);
        }
      }
    }
  }
}
