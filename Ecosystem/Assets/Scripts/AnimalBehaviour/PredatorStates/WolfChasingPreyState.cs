using UnityEngine;

namespace Ecosystem.AnimalBehaviour.PredatorStates
{
  internal sealed class WolfChasingPreyState : AbstractAnimalState
  {
    public WolfChasingPreyState(StateData data)
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
      if (!Target || !Target.activeSelf || !MovementController.IsTargetInRange(Target.transform.position))
      {
        return base.Tick();
      }

      if (Consumer.IsAttacking)
      {
        return AnimalState.Attacking;
      }
      else
      {
        MovementController.UpdateHunting(Target.transform.position);
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
      return AnimalState.ChasingPrey;
    }
  }
}