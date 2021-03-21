using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public class WolfDrinkingState : AbstractAnimalState
  {
    public WolfDrinkingState(WolfStateData data)
    {
      consumer = data.consumer;
      waterConsumer = data.waterConsumer;
      movementController = data.movementController;
      animationController = data.animationController;
      memoryController = data.memoryController;
    }
    
    public override void Begin(GameObject target)
    {
      _target = target;
      movementController.StandStill(true);
      animationController.IdleAnimation();
      waterConsumer.StartDrinking();
    }

    override public AnimalState Tick()
    {
      if (waterConsumer.IsDrinking)
      {
        return this.Type();
      }
      else
      {
        return base.Tick();
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.Drinking;
    }
  }
}