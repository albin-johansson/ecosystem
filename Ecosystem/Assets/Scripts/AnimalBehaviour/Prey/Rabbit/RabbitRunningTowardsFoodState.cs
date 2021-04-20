using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Rabbit
{
  internal sealed class RabbitRunningTowardsFoodState : AbstractAnimalState
  {
    internal RabbitRunningTowardsFoodState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = target;
      MovementController.SetDestinationIfValid(Target.transform.position);
      AnimationController.EnterMoveAnimation();
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
        MovementController.SetDestinationIfValid(Target.transform.position);
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