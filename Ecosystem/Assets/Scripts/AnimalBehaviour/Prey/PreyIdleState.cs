using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey
{
  internal sealed class PreyIdleState : AbstractAnimalState
  {
    internal PreyIdleState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = null;
      AnimationController.EnterIdleAnimation();
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        return AnimalState.Fleeing;
      }
      else
      {
        return base.Tick();
      }
    }

    public override void OnSphereEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsWater(otherObject))
      {
        MemoryController.SaveToMemory(otherObject);
      }
      else if (Tags.IsPredator(otherObject))
      {
        Target = otherObject;
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.Idle;
    }
  }
}