using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public class WolfLookingForMateState : AbstractAnimalState
  {
    public WolfLookingForMateState(WolfStateData data)
    {
      Consumer = data.Consumer;
      WaterConsumer = data.WaterConsumer;
      MovementController = data.MovementController;
      AnimationController = data.AnimationController;
      MemoryController = data.MemoryController;
      Reproducer = data.Reproducer;
    }

    public override void Begin(GameObject target)
    {
      Target = null;
      Reproducer.isWilling = true;
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForMate;
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        if (Reproducer.CompatibleAsParents(Target))
        {
          MovementController.RunToTarget(Target.transform.position);
        }
        else
        {
          Target = null;
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
      
      if (otherObject.CompareTag("Water"))
      {
        MemoryController.SaveToMemory(otherObject);
      }
      else if (Reproducer.CompatibleAsParents(otherObject))
      {
        Target = otherObject;
      }
    }
  }
}