using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Deer
{
  internal sealed class DeerLookingForMateState : AbstractAnimalState
  { 
    public DeerLookingForMateState(StateData data)
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
      Target = GetClosestMateInVision(Layers.DeerMask);
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
        if (!Target.activeSelf)
        {
          Target = GetClosestMateInVision(Layers.DeerMask);
          if (!Target)
          {
            return base.Tick();
          }
        }
        else if (Tags.IsPredator(Target))
        {
          return AnimalState.Fleeing;
        }
        else if (Reproducer.CompatibleAsParents(Target))
        {
          MovementController.RunToTarget(Target.transform.position);
        }
        else
        {
          Target = GetClosestMateInVision(Layers.DeerMask);
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
      else if (Reproducer.CompatibleAsParents(otherObject) || Tags.IsPredator(otherObject))
      {
        Target = otherObject;
      }
    }

    public override void OnTriggerExit(Collider other)
    {
      if (other.gameObject == Target)
      {
        Target = GetClosestMateInVision(Layers.DeerMask);
      }
    }
  }
}