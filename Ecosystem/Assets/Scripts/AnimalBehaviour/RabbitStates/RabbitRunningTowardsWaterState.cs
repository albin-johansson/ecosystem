using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitRunningTowardsWaterState : AbstractAnimalState
  {
    public RabbitRunningTowardsWaterState(RabbitStateData data)
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
      movementController.RunToTarget(_target.transform.position);
      animationController.MoveAnimation();
      //TODO Check memory
    }

    public override AnimalState Tick()
    { 
      if (_target == null)
      {
        return base.Tick();
      }
      
      if(_target.tag == "Wolf" || _target.tag == "Bear")
      {
        return AnimalState.Fleeing;
      }

      if(waterConsumer.CanDrink)
      {
        return AnimalState.Drinking;
      }
      movementController.RunToTarget(_target.transform.position);
      return Type();
    }

    public override AnimalState Type()
    {
      return AnimalState.RunningTowardsWater;
    }

    public override void OnTriggerEnter(Collider other)
      {
      var tag = other.gameObject.tag;
      if (movementController.IsReachable(other.gameObject.transform.position))
      {
        if (tag == "Water")
        {
          //TODO: Check if new source is closer
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
          return;
        }
      }
    }
  }
}
