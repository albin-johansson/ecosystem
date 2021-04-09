using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  internal sealed class WolfLookingForWaterState : AbstractAnimalState
  {
    public WolfLookingForWaterState(WolfStateData data)
    {
      StaminaController = data.StaminaController;
      Consumer = data.Consumer;
      WaterConsumer = data.WaterConsumer;
      MovementController = data.MovementController;
      AnimationController = data.AnimationController;
      MemoryController = data.MemoryController;
      Reproducer = data.Reproducer;
    }

    public override void Begin(GameObject target)
    {
      Target = GetClosestInVision(Layers.WaterMask);
      if (!Target)
      {
        Target = MemoryController.GetClosestInMemory(Tags.IsWater, MovementController.GetPosition());
      }
      MovementController.StartWander();
      AnimationController.EnterMoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        return AnimalState.RunningTowardsWater;
      }
      MovementController.UpdateWander();
      return base.Tick();
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsWater(otherObject))
      {
        MemoryController.SaveToMemory(otherObject);
        Target = otherObject;
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForWater;
    }
  }
}