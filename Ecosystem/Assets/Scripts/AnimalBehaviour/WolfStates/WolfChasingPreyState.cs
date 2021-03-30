using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  internal sealed class WolfChasingPreyState : AbstractAnimalState
  {
    public WolfChasingPreyState(WolfStateData data)
    {
      StaminaController = data.StaminaController;
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
    }

    public override AnimalState Tick()
    {
      if (!Target || !Target.activeSelf || !MovementController.IsTargetInRange(Target.transform.position))
      {
        return base.Tick();
      }

      if (Consumer.IsAttacking)
      {
        AnimationController.MoveAnimation();
        return AnimalState.Attacking;
      }
      else
      {
        if (StaminaController.CanRun())
        {
          StaminaController.IsRunning = true;
          AnimationController.RunAnimation();
        }
        else
        {
          AnimationController.MoveAnimation();
        }
        MovementController.UpdateHunting(Target.transform.position);
      }

      return Type();
    }

    public override GameObject End()
    {
      StaminaController.IsRunning = false;
      return base.End();
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