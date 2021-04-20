using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey
{
  internal sealed class PreyDrinkingState : AbstractAnimalState
  {
    internal PreyDrinkingState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = target;
      AnimationController.EnterIdleAnimation();
      MovementController.SetStandingStill(true);
      WaterConsumer.StartDrinking();
      // TODO: Add animationController.DrinkAnimation();
    }

    public override AnimalState Tick()
    {
      if (Tags.IsPredator(Target))
      {
        return AnimalState.Fleeing;
      }
      else if (WaterConsumer.IsDrinking)
      {
        return Type();
      }
      else
      {
        return base.Tick();
      }
    }

    public override void OnSphereEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsWater(otherObject))
      {
        MemoryController.SaveToMemory(otherObject);
      }
      else if (Tags.IsPredator(otherObject))
      {
        Target = otherObject;
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