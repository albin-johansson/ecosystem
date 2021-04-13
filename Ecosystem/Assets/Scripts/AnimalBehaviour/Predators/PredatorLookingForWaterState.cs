using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators
{
  internal sealed class PredatorLookingForWaterState : AbstractAnimalState
  {
    internal PredatorLookingForWaterState(StateData data) : base(data)
    {
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