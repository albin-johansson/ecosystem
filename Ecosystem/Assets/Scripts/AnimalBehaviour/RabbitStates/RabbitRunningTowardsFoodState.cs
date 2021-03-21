using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitRunningTowardsFoodState : AbstractAnimalState
  {
    public RabbitRunningTowardsFoodState(RabbitStateData data)
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

      if (_target.tag == "Wolf" || _target.tag == "Bear")
      {
        return AnimalState.Fleeing;
      }
      movementController.RunToTarget(_target.transform.position);
      return Type();
    }

    public override AnimalState Type()
    {
      return AnimalState.RunningTowardsFood;
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

        if (tag == "Predator")
        {
          _target = other.gameObject;
          return;
        }

        if (tag == "Food")
        {
          //TODO: Check if the new food is closer than the current target
          memoryController.SaveToMemory(other.gameObject); 
          return;
        }
      }
    }
  }
}
