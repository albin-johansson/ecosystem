using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators
{
  internal sealed class PredatorAttackingState : AbstractAnimalState
  {
    internal PredatorAttackingState(StateData data) : base(data)
    {
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