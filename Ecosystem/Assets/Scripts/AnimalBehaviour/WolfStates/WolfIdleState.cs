using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  internal sealed class WolfIdleState : AbstractAnimalState
  {
    public WolfIdleState(WolfStateData data)
    {
      Consumer = data.consumer;
      WaterConsumer = data.waterConsumer;
      MovementController = data.movementController;
      AnimationController = data.animationController;
      MemoryController = data.memoryController;
    }

    public override void Begin(GameObject target)
    {
      MovementController.StartWander();
    }

    public override AnimalState Tick()
    {
      MovementController.UpdateWander();
      return base.Tick();
    }

    public override AnimalState Type()
    {
      return AnimalState.Idle;
    }
  }
}
