using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public class WolfRunningTowardsWaterState : AbstractAnimalState
  {
    public WolfRunningTowardsWaterState(WolfStateData data)
    {
      consumer = data.consumer;
      waterConsumer = data.waterConsumer;
      movementController = data.movementController;
      animationController = data.animationController;
      memoryController = data.memoryController;
    }
    public override void Begin(GameObject target)
    {
      _target = target;
      movementController.RunToTarget(_target.transform.position);
      animationController.MoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (_target == null)
      {
        return AnimalState.LookingForWater;
      }

      if (waterConsumer.CanDrink)
      {
        return AnimalState.Drinking;
      }

      if (waterConsumer.Thirst < consumer.Hunger && consumer.IsHungry()) //If we are more thirsty then hungry: Look for water
      {
        return AnimalState.LookingForPrey;
      }

      movementController.RunToTarget(_target.transform.position);
      return this.Type();
    }

    public override AnimalState Type()
    {
      return AnimalState.RunningTowardsWater;
    }
  }
}
