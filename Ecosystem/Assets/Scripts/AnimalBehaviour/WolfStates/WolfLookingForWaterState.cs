using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public class WolfLookingForWaterState : AbstractAnimalState
  {
    public WolfLookingForWaterState(WolfStateData data)
    {
      Consumer = data.consumer;
      WaterConsumer = data.waterConsumer;
      MovementController = data.movementController;
      AnimationController = data.animationController;
      MemoryController = data.memoryController;
    }

    public override void Begin(GameObject target)
    {
      Target = null;
      MovementController.StartWander();
      AnimationController.MoveAnimation();
      //TODO Check memory
    }

    public override AnimalState Tick()
    {
      MovementController.UpdateWander();
      if (Target != null)
      {
        return AnimalState.RunningTowardsWater;
      }

      if (WaterConsumer.Thirst < Consumer.Hunger  && Consumer.IsHungry()) //If we are more thirsty then hungry: Look for water
      {
        return AnimalState.LookingForPrey;
      }

      MovementController.UpdateWander();
      return this.Type();
    }

    override public void OnTriggerEnter(Collider other)
    {
      if (MovementController.IsReachable(other.gameObject.transform.position))
      {
        if (other.gameObject.CompareTag("Water"))
        {
          MemoryController.SaveToMemory(other.gameObject);
          Target = other.gameObject;
          return;
        }
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForWater;
    }
  }
}
