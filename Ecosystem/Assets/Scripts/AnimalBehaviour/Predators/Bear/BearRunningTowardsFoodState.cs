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
      AnimationController.EnterMoveAnimation();
      Target = target;
      MovementController.SetDestination(Target.transform.position);
      Consumer.CheckLastCollision();
    }

    public override AnimalState Tick()
    {
      if (Consumer.EatingFromGameObject)
      {
        return AnimalState.Eating;
      }

      if (!Consumer.IsHungry() ||
          !Target ||
          !Target.activeInHierarchy ||
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