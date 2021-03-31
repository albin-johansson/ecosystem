using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  internal sealed class WolfLookingForPreyState : AbstractAnimalState
  {
    public WolfLookingForPreyState(WolfStateData data)
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
      MovementController.StartWander();
      AnimationController.MoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        if (Tags.IsMeat(Target))
        {
          return AnimalState.GoingToMeat;
        }
        else
        {
          return AnimalState.ChasingPrey;
        }
      }
      else if (WaterConsumer.Thirst > Consumer.Hunger && WaterConsumer.IsThirsty())
      {
        return AnimalState.LookingForWater;
      }

      if (Consumer.IsAttacking)
      {
        return AnimalState.Attacking;
      }
      else
      {
        MovementController.UpdateWander();
        return Type();
      }
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (MovementController.IsReachable(otherObject.transform.position))
      {
        if (otherObject.CompareTag("Water"))
        {
          MemoryController.SaveToMemory(otherObject);
        }
        else if (Tags.IsPrey(otherObject) || Tags.IsMeat(otherObject))
        {
          Target = otherObject;
        }
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForPrey;
    }
  }
}