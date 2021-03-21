using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitFleeingState : AbstractAnimalState
  {
    public RabbitFleeingState(RabbitStateData data)
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
      movementController.StartFleeing(_target.transform.position);
      animationController.MoveAnimation();
      //TODO Check memory
    }

    public override AnimalState Tick()
    {
      if(_target == null)
      {
        return base.Tick();
      }
      movementController.UpdateFleeing(_target.transform.position);
      return Type();
    }

    public override AnimalState Type()
    {
      return AnimalState.Fleeing;
    }

    public override void OnTriggerEnter(Collider other)
    {
      if (movementController.IsReachable(other.gameObject.transform.position))
      {
        if (other.gameObject.tag == "Water")
        {
          memoryController.SaveToMemory(other.gameObject);
          return;
        }

        if (other.gameObject.tag == "Predator")
        {
          //TODO: Add logic for tracking all Predators in collision
          return;
        }
      }
    }  
    public override void OnTriggerExit(Collider other)
    {
      if (other.gameObject == _target)  //TODO: Add logic for looking for other predators already in the collider and set the closest one to current target.
      {
        _target = null;                         
      }
    }
  }
}
