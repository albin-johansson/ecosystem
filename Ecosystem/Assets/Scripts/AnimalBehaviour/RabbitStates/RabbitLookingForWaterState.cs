using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  internal sealed class RabbitLookingForWaterState : AbstractAnimalState
  {
    public RabbitLookingForWaterState(RabbitStateData data)
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
      MovementController.StartWander();
      AnimationController.MoveAnimation();
      //TODO Check memory
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        if (Tags.IsPredator(Target))
        {
          return AnimalState.Fleeing;
        }
        else if (Target.CompareTag("Water"))
        {
          return AnimalState.RunningTowardsWater;
        }
      }

      if (Consumer.Hunger > WaterConsumer.Thirst && Consumer.IsHungry())
      {
        return AnimalState.LookingForFood;
      }

      MovementController.UpdateWander();

      return Type();
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;

      if (MovementController.IsReachable(otherObject.transform.position))
      {
        if (otherObject.CompareTag("Water"))
        {
          MemoryController.SaveToMemory(otherObject);
          Target = otherObject;
        }
        else if (Tags.IsPredator(otherObject))
        {
          Target = other.gameObject;
        }
        else if (Tags.IsFood(otherObject))
        {
          MemoryController.SaveToMemory(other.gameObject);
        }
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForWater;
    }
  }
}