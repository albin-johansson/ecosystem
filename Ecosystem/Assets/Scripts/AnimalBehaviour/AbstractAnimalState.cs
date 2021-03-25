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
    protected Reproducer Reproducer;
    protected GameObject Target;

    public abstract AnimalState Type();

    public virtual AnimalState Tick()
    {
      if (Consumer.Hunger > WaterConsumer.Thirst && Consumer.IsHungry())
      {
        return AnimalState.LookingForFood;
      }
      else if (WaterConsumer.Thirst > Consumer.Hunger && WaterConsumer.IsThirsty())
      {
        return AnimalState.LookingForWater;
      }
      else if (Reproducer.IsFertile)
      {
        return AnimalState.LookingForMate;
      }
      else
      {
        return AnimalState.Idle;
      }
    }

    public virtual void Begin(GameObject target)
    {
    }

    public virtual GameObject End()
    {
      Consumer.CollideActive = false;
      return Target;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
      MemoryController.SaveToInVision(other.gameObject);
      if (other.gameObject.CompareTag("Water"))
      {
      }
    }

    public virtual void OnTriggerExit(Collider other)
    {
    }
  }
}