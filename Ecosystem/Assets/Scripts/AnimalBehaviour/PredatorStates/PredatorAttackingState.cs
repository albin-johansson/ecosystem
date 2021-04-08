using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.PredatorStates
{
  internal sealed class PredatorAttackingState : AbstractAnimalState
  {
    public PredatorAttackingState(StateData data)
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
      AnimationController.EnterAttackAnimation();
      MovementController.StandStill(true);
    }

    public override AnimalState Tick()
    {
      if (AnimationController.IsIdle())
      {
        Consumer.IsAttacking = false;
        return base.Tick();
      }

      return Type();
    }

    public override AnimalState Type()
    {
      return AnimalState.Attacking;
    }
  }
}