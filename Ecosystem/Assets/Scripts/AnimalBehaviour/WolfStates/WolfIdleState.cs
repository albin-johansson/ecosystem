using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public class WolfIdleState : AbstractAnimalState
  {
    public WolfIdleState(WolfStateData data)
    {
      consumer = data.consumer;
      waterConsumer = data.waterConsumer;
      movementController = data.movementController;
      animationController = data.animationController;
      memoryController = data.memoryController;
    }

    public override void Begin(GameObject target)
    {
      movementController.StartWander();
    }

    public override AnimalState Tick()
    {
      movementController.UpdateWander();
      return base.Tick();
    }


    public override AnimalState Type()
    {
      return AnimalState.Idle;
    }
  }
}
