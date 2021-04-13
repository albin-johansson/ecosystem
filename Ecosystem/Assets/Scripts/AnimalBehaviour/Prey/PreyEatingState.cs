using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey
{
  internal sealed class PreyEatingState : AbstractAnimalState
  {
    internal PreyEatingState(StateData data) : base(data)
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
      if (Tags.IsPredator(Target))
      {
        return AnimalState.Fleeing;
      }

      if (Consumer.EatingFromGameObject)
      {
        return Type();
      }
      else
      {
        return base.Tick();
      }
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsPredator(otherObject))
      {
        Target = otherObject;
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.Eating;
    }
  }
}