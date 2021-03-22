using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public class WolfChasingPreyState : AbstractAnimalState
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
      if (Target == null || !MovementController.IsTargetInRange(Target.transform.position))
      {
        return base.Tick();
      }
      else
      {
        MovementController.UpdateHunting(Target.transform.position);
      }

      if (WaterConsumer.Thirst > Consumer.Hunger && WaterConsumer.IsThirsty()) //If we are more thirsty then hungry: Look for water
      {
        return AnimalState.LookingForWater;
      }


      return this.Type();
    }

    override public void OnTriggerExit(Collider other)
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
