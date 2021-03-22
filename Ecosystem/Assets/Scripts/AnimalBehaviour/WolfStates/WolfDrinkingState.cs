using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public class WolfDrinkingState : AbstractAnimalState
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

    override public AnimalState Tick()
    {
      if (WaterConsumer.IsDrinking)
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