using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  internal sealed class RabbitDrinkingState : AbstractAnimalState
  {
    public RabbitDrinkingState(RabbitStateData data)
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
      Target = target;
      MovementController.StandStill(true);
      WaterConsumer.StartDrinking();
      AnimationController.IdleAnimation();
      // TODO: Add animationController.DrinkAnimation();
      //TODO Check memory
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

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (MovementController.IsReachable(otherObject.transform.position))
      {
        if (otherObject.CompareTag("Water") || Tags.IsFood(otherObject))
        {
          MemoryController.SaveToMemory(otherObject);
        }
        else if (Tags.IsPredator(otherObject))
        {
          Target = otherObject;
        }
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.Drinking;
    }
  }
}