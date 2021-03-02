using UnityEngine;

namespace Ecosystem
{
  public sealed class ResourceFinder : MonoBehaviour
  {
    [SerializeField] private FoodConsumer foodConsumer;
    [SerializeField] private WaterConsumer waterConsumer;
    [SerializeField] private MateFinder mateFinder;
    [SerializeField] private MemoryController memoryController;
    [SerializeField] private TargetTracker targetTracker;

    private Desire _priority = Desire.Idle;

    private void Update()
    {
      UpdatePriority();
      CheckMemory();
    }

    //Checks the memory for objects that matches the prioritised desire
    private void CheckMemory()
    {
      if (!targetTracker.HasTarget && _priority != Desire.Idle)
      {
        var (match, vector3) = memoryController.GetFromMemory(_priority);
        if (match)
        {
          targetTracker.SetTarget(vector3, _priority);
        }
      }
    }

    //Sets priority, will set priority of what is currently most needed.
    private void UpdatePriority()
    {
      // TODO in the future, we might want to use Desire.Mate here
      if (foodConsumer.Hunger > waterConsumer.Thirst && foodConsumer.IsHungry())
      {
        _priority = Desire.Food;
      }
      else if (waterConsumer.Thirst > foodConsumer.Hunger && waterConsumer.IsThirsty())
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
    /// If a predator is within field of view the animal will flee.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
      if (LayerUtil.IsPredatorLayer(other.gameObject.layer))
      {
        targetTracker.FleeFromPredator(other.gameObject);
        return;
      }

      memoryController.SaveToMemory(other.gameObject);

      if (!targetTracker.HasTarget)
      {
        if (_priority == Desire.Food && other.gameObject.layer == LayerUtil.FoodLayer ||
            _priority == Desire.Water && other.gameObject.layer == LayerUtil.WaterLayer ||
            _priority == Desire.Idle && mateFinder.CompatibleAsParents(other.gameObject))
        {
          targetTracker.SetTarget(other.gameObject.transform.position, _priority);
        }
      }
    }
  }
}