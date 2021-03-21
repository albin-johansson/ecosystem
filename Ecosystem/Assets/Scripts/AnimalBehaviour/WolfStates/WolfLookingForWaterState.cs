using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public class WolfLookingForWaterState : AbstractAnimalState
  {
    public WolfLookingForWaterState(WolfStateData data)
    {
      consumer = data.consumer;
      waterConsumer = data.waterConsumer;
      movementController = data.movementController;
      animationController = data.animationController;
      memoryController = data.memoryController;
    }

    public override void Begin(GameObject target)
    {
      _target = null;
      movementController.StartWander();
      animationController.MoveAnimation();
      //TODO Check memory
    }

    public override AnimalState Tick()
    {
      movementController.UpdateWander();
      if (_target != null)
      {
        return AnimalState.RunningTowardsWater;
      }

      if (waterConsumer.Thirst < consumer.Hunger  && consumer.IsHungry()) //If we are more thirsty then hungry: Look for water
      {
        return AnimalState.LookingForPrey;
      }

      movementController.UpdateWander();
      return this.Type();
    }

    override public void OnTriggerEnter(Collider other)
    {
      if (movementController.IsReachable(other.gameObject.transform.position))
      {
        if (other.gameObject.CompareTag("Water"))
        {
          memoryController.SaveToMemory(other.gameObject);
          _target = other.gameObject;
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
