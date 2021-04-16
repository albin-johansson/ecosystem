using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators
{
  internal sealed class PredatorDrinkingState : AbstractAnimalState
  {
    internal PredatorDrinkingState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = target;
      MovementController.SetStandingStill(true);
      AnimationController.EnterIdleAnimation();
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