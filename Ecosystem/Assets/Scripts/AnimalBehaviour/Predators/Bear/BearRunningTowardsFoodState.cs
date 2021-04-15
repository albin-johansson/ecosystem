using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Bear
{
  internal sealed class BearRunningTowardsFoodState : AbstractAnimalState
  {
    internal BearRunningTowardsFoodState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = target;
      MovementController.SetDestination(Target.transform.position);
      AnimationController.EnterMoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (Consumer.EatingFromGameObject)
      {
        return AnimalState.Eating;
      }
      if (!Consumer.IsHungry() ||
          !Target ||
          !Target.activeSelf ||
          !MovementController.IsWithinSphere(Target.transform.position))
      {
        return base.Tick();
      }

      return Type();
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
      return AnimalState.RunningTowardsFood;
    }
  }
}