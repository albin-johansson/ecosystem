using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  internal sealed class WolfLookingForWaterState : AbstractAnimalState
  {
    public WolfLookingForWaterState(WolfStateData data)
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

      // TODO Check memory
    }

    public override AnimalState Tick()
    {
      MovementController.UpdateWander();
      if (Target)
      {
        return AnimalState.RunningTowardsWater;
      }
      else if (WaterConsumer.Thirst < Consumer.Hunger && Consumer.IsHungry())
      {
        return AnimalState.LookingForPrey;
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
          Target = otherObject;
        }
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForWater;
    }
  }
}