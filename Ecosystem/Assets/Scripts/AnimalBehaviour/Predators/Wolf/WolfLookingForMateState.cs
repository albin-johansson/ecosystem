using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Wolf
{
  public class WolfLookingForMateState : AbstractAnimalState
  {
    internal WolfLookingForMateState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Reproducer.IsWilling = true;
      AnimationController.EnterMoveAnimation();
      Target = GetClosestMateInVision(Layers.WolfMask);
      MovementController.StartWander();
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForMate;
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        if (Reproducer.CompatibleAsParents(Target) &&
            Reproducer.CanMate &&
            MovementController.IsWithinSphere(Target.transform.position))
        {
          MovementController.SetDestinationIfValid(Target.transform.position);
          return Type();
        }
        else
        {
          Target = GetClosestMateInVision(Layers.WolfMask);
        }
      }
      else
      {
        MovementController.UpdateWander();
      }

      return base.Tick();
    }

    public override GameObject End()
    {
      Reproducer.IsWilling = false;
      return base.End();
    }

    public override void OnSphereEnter(Collider other)
    {
      var otherObject = other.gameObject;

      if (Tags.IsWater(otherObject))
      {
        MemoryController.SaveToMemory(otherObject);
      }
      else if (Reproducer.CompatibleAsParents(otherObject))
      {
        Target = otherObject;
      }
    }

    public override void OnSphereExit(Collider other)
    {
      if (other.gameObject == Target)
      {
        Target = GetClosestMateInVision(Layers.WolfMask);
      }
    }
  }
}
