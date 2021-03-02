using UnityEngine;

namespace Ecosystem
{
  public sealed class PreyFinder : MonoBehaviour
  {
    [SerializeField] private PreyConsumer preyConsumer;
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
      if (preyConsumer.Hunger > waterConsumer.Thirst && preyConsumer.IsHungry())
      {
        _priority = Desire.Prey;
      }
      else if (waterConsumer.Thirst > preyConsumer.Hunger && waterConsumer.IsThirsty())
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
    private void OnTriggerEnter(Collider other)
    {
      memoryController.SaveToMemory(other.gameObject);

      if (!targetTracker.HasTarget)
      {
        if (_priority == Desire.Prey && other.gameObject.layer == LayerUtil.PreyLayer ||
            _priority == Desire.Water && other.gameObject.layer == LayerUtil.WaterLayer ||
            _priority == Desire.Idle && mateFinder.CompatibleAsParents(other.gameObject))
        {
          targetTracker.SetTarget(other.gameObject.transform.position, _priority);
        }
      }
    }
  }
}