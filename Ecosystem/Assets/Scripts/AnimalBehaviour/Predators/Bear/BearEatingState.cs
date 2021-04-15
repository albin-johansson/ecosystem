using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Bear
{
  internal sealed class BearEatingState : AbstractAnimalState
  {
    internal BearEatingState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = target;
      AnimationController.EnterIdleAnimation();
      MovementController.SetStandingStill(true);
    }

    public override AnimalState Tick()
    {
      if (Consumer.EatingFromGameObject)
      {
        return Type();
      }
      else
      {
        return base.Tick();
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.Eating;
    }
  }
}