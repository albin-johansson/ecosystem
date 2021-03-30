using UnityEngine;

namespace Ecosystem.AnimalBehaviour.PredatorStates
{
  internal sealed class PredatorRunningTowardsWaterState : AbstractAnimalState
  {
    public PredatorRunningTowardsWaterState(StateData data)
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
      Target = target;
      MovementController.RunToTarget(Target.transform.position);
      AnimationController.MoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (!Target)
      {
        return base.Tick();
      }
      else if (WaterConsumer.CanDrink)
      {
        return AnimalState.Drinking;
      }
      else if (WaterConsumer.Thirst < Consumer.Hunger && Consumer.IsHungry())
      {
        return AnimalState.LookingForPrey;
      }
      else
      {
        MovementController.RunToTarget(Target.transform.position);
        return Type();
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.RunningTowardsWater;
    }
  }
}