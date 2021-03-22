using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  internal sealed class WolfDrinkingState : AbstractAnimalState
  {
    public WolfDrinkingState(WolfStateData data)
    {
      Consumer = data.consumer;
      WaterConsumer = data.waterConsumer;
      MovementController = data.movementController;
      AnimationController = data.animationController;
      MemoryController = data.memoryController;
    }

    public override void Begin(GameObject target)
    {
      Target = target;
      MovementController.StandStill(true);
      AnimationController.IdleAnimation();
      WaterConsumer.StartDrinking();
    }

    public override AnimalState Tick()
    {
      return WaterConsumer.IsDrinking ? Type() : base.Tick();
    }

    public override AnimalState Type()
    {
      return AnimalState.Drinking;
    }
  }
}