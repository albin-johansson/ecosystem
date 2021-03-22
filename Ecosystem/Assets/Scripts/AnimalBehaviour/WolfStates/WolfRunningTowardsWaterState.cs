using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public class WolfRunningTowardsWaterState : AbstractAnimalState
  {
    public WolfRunningTowardsWaterState(WolfStateData data)
    {
      Consumer = data.consumer;
      WaterConsumer = data.waterConsumer;
      MovementController = data.movementController;
      AnimationController = data.animationController;
      MemoryController = data.memoryController;
    }
    public override void Begin(GameObject target)
    {
      Target = target;
      MovementController.RunToTarget(Target.transform.position);
      AnimationController.MoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (Target == null)
      {
        return AnimalState.LookingForWater;
      }

      if (WaterConsumer.CanDrink)
      {
        return AnimalState.Drinking;
      }

      if (WaterConsumer.Thirst < Consumer.Hunger && Consumer.IsHungry()) //If we are more thirsty then hungry: Look for water
      {
        return AnimalState.LookingForPrey;
      }

      MovementController.RunToTarget(Target.transform.position);
      return this.Type();
    }

    public override AnimalState Type()
    {
      return AnimalState.RunningTowardsWater;
    }
  }
}
