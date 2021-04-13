using System;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Bear
{
  internal sealed class BearRunningTowardsFoodState : AbstractAnimalState
  {
    public BearRunningTowardsFoodState(StateData data)
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
      MovementController.StartHunting(Target.transform.position);
      AnimationController.MoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (Consumer.EatingFromGameObject)
      {
        return AnimalState.Eating;
      }
      if (!Consumer.IsHungry() || !Target || !Target.activeSelf || !MovementController.IsTargetInRange(Target.transform.position))
      {
        return base.Tick();
      }
      return Type();
    }

    public override void OnTriggerExit(Collider other)
    {
      if (other.gameObject == Target)
      {
        Target = null;
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.RunningTowardsFood;
    }
  }
}