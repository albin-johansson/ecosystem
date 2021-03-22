using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  internal sealed class RabbitRunningTowardsWaterState : AbstractAnimalState
  {
    public RabbitRunningTowardsWaterState(RabbitStateData data)
    {
      Consumer = data.Consumer;
      WaterConsumer = data.WaterConsumer;
      MovementController = data.MovementController;
      AnimationController = data.AnimationController;
      MemoryController = data.MemoryController;
    }

    public override void Begin(GameObject target)
    {
      Target = target;
      MovementController.RunToTarget(Target.transform.position);
      AnimationController.MoveAnimation();
      //TODO Check memory
    }

    public override AnimalState Tick()
    {
      if (!Target)
      {
        return base.Tick();
      }
      else if (Tags.IsPredator(Target))
      {
        return AnimalState.Fleeing;
      }
      else if (WaterConsumer.CanDrink)
      {
        return AnimalState.Drinking;
      }
      else
      {
        MovementController.RunToTarget(Target.transform.position);
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
          // TODO: Check if new source is closer
          MemoryController.SaveToMemory(otherObject);
        }
        else if (Tags.IsPredator(otherObject))
        {
          Target = otherObject;
        }
        else if (Tags.IsFood(otherObject))
        {
          MemoryController.SaveToMemory(otherObject);
        }
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.RunningTowardsWater;
    }
  }
}