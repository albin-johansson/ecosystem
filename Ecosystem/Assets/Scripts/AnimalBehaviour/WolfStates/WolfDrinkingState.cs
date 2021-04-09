using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  internal sealed class WolfDrinkingState : AbstractAnimalState
  {
    public WolfDrinkingState(WolfStateData data)
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
      Target = target;
      MovementController.SetStandingStill(true);
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
        MovementController.SetStandingStill(false);
        return base.Tick();
      }
    }

    public override GameObject End()
    {
      WaterConsumer.StopDrinking();
      return base.End();
    }

    public override AnimalState Type()
    {
      return AnimalState.Drinking;
    }
  }
}