using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators
{
  internal sealed class PredatorIdleState : AbstractAnimalState
  {
    internal PredatorIdleState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = null;
      AnimationController.EnterIdleAnimation();
    }

    public override AnimalState Type()
    {
      return AnimalState.Idle;
    }
  }
}