using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitFleeingState : AbstractAnimalState
  {
    public RabbitFleeingState(RabbitStateData data)
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
      MovementController.StartFleeing(Target.transform.position);
      AnimationController.MoveAnimation();
      //TODO Check memory
    }

    public override AnimalState Tick()
    {
      if(Target == null)
      {
        return base.Tick();
      }
      MovementController.UpdateFleeing(Target.transform.position);
      return Type();
    }

    public override AnimalState Type()
    {
      return AnimalState.Fleeing;
    }

    public override void OnTriggerEnter(Collider other)
    {
      if (MovementController.IsReachable(other.gameObject.transform.position))
      {
        var tag = other.gameObject.tag;
        if (tag == "Water")
        {
          MemoryController.SaveToMemory(other.gameObject);
          return;
        }

        if (tag == "Wolf" || tag == "Bear")
        {
          //TODO: Add logic for tracking all Predators in collision
          return;
        }
      }
    }  
    public override void OnTriggerExit(Collider other)
    {
      if (other.gameObject == Target)  //TODO: Add logic for looking for other predators already in the collider and set the closest one to current target.
      {
        Target = null;                         
      }
    }
  }
}
