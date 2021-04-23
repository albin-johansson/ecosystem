using Ecosystem.Util;
using UnityEditor;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Rabbit
{
  public class RabbitLookingForMateState : AbstractAnimalState
  {
    internal RabbitLookingForMateState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Reproducer.IsWilling = true;
      AnimationController.EnterMoveAnimation();
      Target = GetClosestMateInVision(Layers.RabbitMask);
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
        if (Tags.IsPredator(Target))
        {
          return AnimalState.Fleeing;
        }
        
        if (!Target.activeInHierarchy || !MovementController.IsWithinSphere(Target.transform.position))
        {
          Target = GetClosestMateInVision(Layers.RabbitMask);
          if (!Target)
          {
            return base.Tick();
          }
        }
        else if (Reproducer.CompatibleAsParents(Target) && Reproducer.CanMate)
        {
          MovementController.SetDestinationIfValid(Target.transform.position);
        }
        else
        {
          Target = GetClosestMateInVision(Layers.RabbitMask);
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
      else if (Reproducer.CompatibleAsParents(otherObject) || Tags.IsPredator(otherObject))
      {
        Target = otherObject;
      }
    }

    public override void OnSphereExit(Collider other)
    {
      if (other.gameObject == Target)
      {
        Target = GetClosestMateInVision(Layers.RabbitMask);
      }
    }
  }
}
