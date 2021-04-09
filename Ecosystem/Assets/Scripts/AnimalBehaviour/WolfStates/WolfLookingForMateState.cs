using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public class WolfLookingForMateState : AbstractAnimalState
  {
    public WolfLookingForMateState(WolfStateData data)
    {
      StaminaController = data.StaminaController;
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
      Reproducer.IsWilling = true;
      AnimationController.EnterMoveAnimation();
      Target = GetClosestMateInVision(Layers.PredatorMask);
      if (Target)
      {
        MovementController.SetDestinationIfValid(target.transform.position);
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
            MovementController.IsWithinSphere(Target.transform.position))
        {
          MovementController.SetDestinationIfValid(Target.transform.position);
          return Type();
        }
        else
        {
          Target = GetClosestMateInVision(Layers.PredatorMask);
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

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;

      if (otherObject.CompareTag("Water"))
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
        Target = GetClosestMateInVision(Layers.PredatorMask);
      }
    }
  }
}