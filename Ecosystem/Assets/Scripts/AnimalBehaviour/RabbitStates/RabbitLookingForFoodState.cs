using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitLookingForFoodState : AbstractAnimalState
  {
    public RabbitLookingForFoodState(RabbitStateData data)
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
        if(_target.tag == "Wolf" || _target.tag == "Bear")
        {
          return AnimalState.Fleeing;
        }

        if(_target.tag == "Food")
        {
          return AnimalState.RunningTowardsFood;
        }
      }
      
      if(consumer.Hunger < waterConsumer.Thirst && waterConsumer.IsThirsty())
      {
        return AnimalState.LookingForWater;
      }
      movementController.UpdateWander();
      return Type();
    }

    public override void OnTriggerEnter(Collider other)
    {
      var tag = other.gameObject.tag;
      if (movementController.IsReachable(other.gameObject.transform.position))
      {
        if (tag == "Water")
        {
          memoryController.SaveToMemory(other.gameObject);
          return;
        }

        if (tag == "Wolf" || tag == "Bear")
        {
          _target = other.gameObject;
          return;
        }

        if (tag == "Food")
        {
          memoryController.SaveToMemory(other.gameObject);
          _target = other.gameObject;
          return;
        }
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForFood;
    }
  }
}
