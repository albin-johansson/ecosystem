using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitLookingForWaterState : AbstractAnimalState
  {
    public RabbitLookingForWaterState(RabbitStateData data)
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
      if(_target != null)
      {
        if(_target.tag == "Predator")
        {
          return AnimalState.Fleeing;
        }

        if(_target.tag == "Water")
        {
          return AnimalState.RunningTowardsWater;
        }
      }
      
      if(consumer.Hunger > waterConsumer.Thirst && consumer.IsHungry())
      {
        return AnimalState.LookingForFood;
      }
      movementController.UpdateWander();
      return Type();
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForWater;
    }

    public override void OnTriggerEnter(Collider other)
    {
      var tag = other.gameObject.tag;
      if (movementController.IsReachable(other.gameObject.transform.position))
      {
        if (tag == "Water")
        {
          memoryController.SaveToMemory(other.gameObject);
          _target = other.gameObject;
          return;
        }

        if (tag == "Predator")
        {
          _target = other.gameObject;
          return;
        }

        if (tag == "Food")
        {
          memoryController.SaveToMemory(other.gameObject); 
          return;
        }
      }
    }
  }
}
