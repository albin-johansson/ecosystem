using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  internal sealed class WolfChasingPreyState : AbstractAnimalState
  {
    public WolfChasingPreyState(WolfStateData data)
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
      MovementController.StartHunting(Target.transform.position);
      AnimationController.MoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (!Target || !MovementController.IsTargetInRange(Target.transform.position))
      {
        return base.Tick();
      }
      else
      {
        MovementController.UpdateHunting(Target.transform.position);
      }

      if (WaterConsumer.Thirst > Consumer.Hunger && WaterConsumer.IsThirsty())
      {
        return AnimalState.LookingForWater;
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
      return AnimalState.LookingForWater;
    }
  }
}