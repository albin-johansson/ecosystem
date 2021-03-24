using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  internal sealed class WolfDrinkingState : AbstractAnimalState
  {
    public WolfDrinkingState(WolfStateData data)
    {
      Consumer = data.Consumer;
      WaterConsumer = data.WaterConsumer;
      MovementController = data.MovementController;
      AnimationController = data.AnimationController;
      MemoryController = data.MemoryController;
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
      if (WaterConsumer.IsDrinking)
      {
        return Type();
      }
      else
      {
        MovementController.StandStill(false);
        return base.Tick();
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.Drinking;
    }
  }
}