using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  internal sealed class RabbitFleeingState : AbstractAnimalState
  {
    public RabbitFleeingState(RabbitStateData data)
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
      Target = target;
      MovementController.FleeFrom(Target.transform.position);
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        if (!Target.activeSelf)
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
          AnimationController.RunAnimation();
        }
        else
        {
          AnimationController.MoveAnimation();
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

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (otherObject.CompareTag("Water"))
      {
        MemoryController.SaveToMemory(otherObject);
      }
    }

    public override void OnTriggerExit(Collider other)
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