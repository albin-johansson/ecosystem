using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey
{
  internal sealed class PreyFleeingState : AbstractAnimalState
  {
    internal PreyFleeingState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = target;
      MovementController.FleeFrom(Target.transform.position);
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        if (!Target.activeInHierarchy || !MovementController.IsWithinSphere(Target.transform.position))
        {
          Target = GetClosestInVision(Layers.PredatorMask);
          if (!Target)
          {
            return base.Tick();
          }
        }

        if (StaminaController.CanRun())
        {
          StaminaController.IsRunning = true;
          AnimationController.EnterRunAnimation();
        }
        else
        {
          AnimationController.EnterMoveAnimation();
        }

        MovementController.UpdateFleeing(Target.transform.position);
        return Type();
      }

      return base.Tick();
    }

    public override GameObject End()
    {
      StaminaController.IsRunning = false;
      return base.End();
    }

    public override void OnSphereEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsWater(otherObject))
      {
        MemoryController.SaveToMemory(otherObject);
      }
    }

    public override void OnSphereExit(Collider other)
    {
      if (other.gameObject == Target)
      {
        Target = GetClosestInVision(Layers.PredatorMask);
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.Fleeing;
    }
  }
}