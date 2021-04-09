using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  internal sealed class WolfAttackingState : AbstractAnimalState
  {
    public WolfAttackingState(WolfStateData data)
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
      MovementController.SetStandingStill(true);
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