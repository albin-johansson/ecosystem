using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Bear
{
  internal sealed class BearChasingPreyState : AbstractAnimalState
  {
    internal BearChasingPreyState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = target;
      MovementController.SetDestination(Target.transform.position);
    }

    public override AnimalState Tick()
    {
      if (Consumer.IsConsuming)
      {
        return AnimalState.Attacking;
      }

      if (!Target || !Consumer.IsHungry())
      {
        return base.Tick();
      }
      else if (!Target.activeInHierarchy ||
               !MovementController.IsWithinSphere(Target.transform.position))
      {
        Target = GetClosestInVision(Layers.PreyMask);
        return Type();
      }
      else
      {
        if (StaminaController.CanRun())
        {
          StaminaController.IsRunning = true;
          AnimationController.EnterRunAnimation();
        }
        else
        {
          AnimationController.EnterMoveAnimation();
        }

        MovementController.SetDestination(Target.transform.position);
      }

      return Type();
    }

    public override GameObject End()
    {
      StaminaController.IsRunning = false;
      return base.End();
    }

    public override void OnSphereEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsPrey(otherObject))
      {
        Target = !Target ? otherObject : SelectCloser(Target, otherObject);
      }
    }

    public override void OnSphereExit(Collider other)
    {
      if (other.gameObject == Target)
      {
        Target = GetClosestInVision(Layers.PreyMask | Layers.MeatMask);
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.ChasingPrey;
    }
  }
}