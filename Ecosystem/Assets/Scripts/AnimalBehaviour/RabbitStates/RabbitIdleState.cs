using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  internal sealed class RabbitIdleState : AbstractAnimalState
  {
    public RabbitIdleState(RabbitStateData data)
    {
      Consumer = data.Consumer;
      WaterConsumer = data.WaterConsumer;
      MovementController = data.MovementController;
      AnimationController = data.AnimationController;
      MemoryController = data.MemoryController;
      Reproducer = data.Reproducer;
    }

    public override void Begin(GameObject target)
    {
      Target = null;
      AnimationController.IdleAnimation();
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        return AnimalState.Fleeing;
      }
      else if (Consumer.Hunger >= WaterConsumer.Thirst && Consumer.IsHungry())
      {
        return AnimalState.LookingForFood;
      }
      else if (WaterConsumer.Thirst > Consumer.Hunger && WaterConsumer.IsThirsty())
      {
        return AnimalState.LookingForWater;
      }
      else
      {
        return Type();
      }
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (MovementController.IsReachable(otherObject.transform.position))
      {
        if (otherObject.CompareTag("Water"))
        {
          MemoryController.SaveToMemory(otherObject);
        }
        else if (Tags.IsPredator(otherObject))
        {
          Target = otherObject;
        }
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.Idle;
    }
  }
}