using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitIdleState : AbstractAnimalState
  {
    public RabbitIdleState(RabbitStateData data)
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

    override public AnimalState Tick()
    {
      if (_target != null)
      {
        return AnimalState.Fleeing;
      }

      if (consumer.Hunger >= waterConsumer.Thirst && consumer.IsHungry())
      {
        return AnimalState.LookingForFood;
      }

      if (waterConsumer.Thirst > consumer.Hunger && waterConsumer.IsThirsty())
      {
        return AnimalState.LookingForWater;
      }

      return Type();
    }

    public override void OnTriggerEnter(Collider other)
    {
      if (movementController.IsReachable(other.gameObject.transform.position))
      {
        var tag = other.gameObject.tag;
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
      }
    }
    
    public override AnimalState Type()
    {
      return AnimalState.Idle;
    }
  }
}
