using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Bear
{
  internal sealed class BearLookingForMateState : AbstractAnimalState
  {
    public BearLookingForMateState(StateData data)
    {
      Consumer = data.Consumer;
      WaterConsumer = data.WaterConsumer;
      MovementController = data.MovementController;
      AnimationController = data.AnimationController;
      MemoryController = data.MemoryController;
      Reproducer = data.Reproducer;
      Genome = data.Genome;
    }

    public override void Begin(GameObject target)
    {
      Reproducer.isWilling = true;
      AnimationController.MoveAnimation();
      Target = GetClosestMateInVision(Layers.BearMask);
      if (Target)
      {
        MovementController.RunToTarget(target.transform.position);
      }
      else
      {
        MovementController.StartWander();
      }
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
            MovementController.IsTargetInRange(Target.transform.position))
        {
          MovementController.RunToTarget(Target.transform.position);
          return Type();
        }
        else
        {
          Target = GetClosestMateInVision(Layers.BearMask);
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
      Reproducer.isWilling = false;
      return base.End();
    }

    public override void OnTriggerEnter(Collider other)
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

    public override void OnTriggerExit(Collider other)
    {
      if (other.gameObject == Target)
      {
        Target = GetClosestMateInVision(Layers.BearMask);
      }
    }
  }
}