using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Deer
{
  internal sealed class DeerRunningTowardsFoodState : AbstractAnimalState
  {
    internal DeerRunningTowardsFoodState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      AnimationController.EnterMoveAnimation();
      Target = target;
      MovementController.SetDestination(Target.transform.position);
    }

    public override AnimalState Tick()
    {
      if (!Target || !Consumer.IsHungry() || !Target.activeInHierarchy)
      {
        return base.Tick();
      }
      else if (Tags.IsPredator(Target))
      {
        return AnimalState.Fleeing;
      }
      else if (Consumer.EatingFromGameObject)
      {
        return AnimalState.Eating;
      }
      else
      {
        MovementController.SetDestination(Target.transform.position);
        return Type();
      }
    }

    public override void OnSphereEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsFood(otherObject))
      {
        Target = SelectCloser(otherObject, Target);
      }
      else if (Tags.IsWater(otherObject))
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
      return AnimalState.RunningTowardsFood;
    }
  }
}