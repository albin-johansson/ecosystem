using UnityEngine;

namespace Ecosystem.AnimalBehaviour
{
  public abstract class AbstractAnimalState : IAnimalState
  {
    protected IConsumer Consumer;
    protected WaterConsumer WaterConsumer;
    protected MemoryController MemoryController;
    protected MovementController MovementController;
    protected EcoAnimationController AnimationController;
    protected GameObject Target;

    public abstract AnimalState Type();

    public virtual AnimalState Tick()
    {
      if (Consumer.Hunger > WaterConsumer.Thirst && Consumer.IsHungry())
      {
        return AnimalState.LookingForPrey;
      }
      else if (WaterConsumer.Thirst > Consumer.Hunger && WaterConsumer.IsThirsty())
      {
        return AnimalState.LookingForWater;
      }
      else
      {
        return AnimalState.Idle;
      }
    }

    public virtual void Begin(GameObject target)
    {
    }

    public GameObject End()
    {
      return Target;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
      if (MovementController.IsReachable(other.gameObject.transform.position) &&
          other.gameObject.CompareTag("Water"))
      {
        MemoryController.SaveToMemory(other.gameObject);
      }
    }

    public virtual void OnTriggerExit(Collider other)
    {
      if (other.gameObject.CompareTag("Water"))
      {
        MemoryController.SaveToMemory(other.gameObject);
      }
    }
  }
}