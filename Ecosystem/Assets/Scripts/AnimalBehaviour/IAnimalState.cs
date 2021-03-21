using UnityEngine;

namespace Ecosystem.AnimalBehaviour
{
  public interface IAnimalState
  {
    void Begin(GameObject target);

    GameObject End();

    AnimalState Tick();

    void OnTriggerEnter(Collider other);

    void OnTriggerExit(Collider other);

    AnimalState Type();
  }

  public abstract class AbstractAnimalState : IAnimalState
  {
    public IConsumer consumer;
    public WaterConsumer waterConsumer;
    public MateFinder mateFinder;
    public MemoryController memoryController;
    public MovementController movementController;
    public EcoAnimationController animationController;
    protected GameObject _target;
    public abstract AnimalState Type();

    public virtual AnimalState Tick()
    {
      if (consumer.Hunger > waterConsumer.Thirst && consumer.IsHungry())  //If we are more hungry then thirsty: Look for food
      {
        return AnimalState.LookingForPrey;
      }
      else if (waterConsumer.Thirst > consumer.Hunger && waterConsumer.IsThirsty()) //If we are more thirsty then hungry: Look for water
      {
        return AnimalState.LookingForWater;
      }

      return AnimalState.Idle;
    }

    public virtual void Begin(GameObject target)
    {
    }

    public virtual GameObject End()
    {
      return _target;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
      if (movementController.IsReachable(other.gameObject.transform.position))
      {
        if (other.gameObject.CompareTag("Water"))
        {
          memoryController.SaveToMemory(other.gameObject);
          return;
        }
      }
    }
    public virtual void OnTriggerExit(Collider other)
    {
      if (other.gameObject.tag == "Water")
      {
        memoryController.SaveToMemory(other.gameObject);
      }
    }
  }
}
