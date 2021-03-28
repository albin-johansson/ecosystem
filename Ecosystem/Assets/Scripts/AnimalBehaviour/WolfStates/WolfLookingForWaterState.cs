using Ecosystem.Util;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  internal sealed class WolfLookingForWaterState : AbstractAnimalState
  {
    private readonly LayerMask _whatIsWater = LayerMask.GetMask("Water");
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
      Target = GetClosestInVision(_whatIsWater);
      if (!Target)
      {
        Target = MemoryController.GetClosestInMemory(Tags.IsWater, MovementController.GetPosition());
      }
      MovementController.StartWander();
      AnimationController.MoveAnimation();
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
