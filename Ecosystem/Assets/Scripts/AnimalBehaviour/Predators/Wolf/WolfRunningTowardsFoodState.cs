using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Wolf
{
  internal sealed class WolfRunningTowardsFoodState : AbstractAnimalState
  {
    internal WolfRunningTowardsFoodState(StateData data) : base(data)
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
      if (!Consumer.IsHungry() ||
          !Target ||
          !Target.activeSelf ||
          !MovementController.IsWithinSphere(Target.transform.position))
      {
        return base.Tick();
      }

      return Type();
    }

    public override void OnSphereExit(Collider other)
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