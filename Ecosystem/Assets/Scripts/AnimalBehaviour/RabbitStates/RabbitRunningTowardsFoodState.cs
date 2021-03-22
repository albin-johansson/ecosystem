using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitRunningTowardsFoodState : AbstractAnimalState
  {
    public RabbitRunningTowardsFoodState(RabbitStateData data)
    {
      Consumer = data.Consumer;
      WaterConsumer = data.WaterConsumer;
      MovementController = data.MovementController;
      AnimationController = data.AnimationController;
      MemoryController = data.MemoryController;
    }
    
    public override void Begin(GameObject target)
    {
      Target = target;
      MovementController.RunToTarget(Target.transform.position);
      AnimationController.MoveAnimation();
      //TODO Check memory
    }

    public override AnimalState Tick()
    {
      if (Target == null)
      {
        return base.Tick();
      }

      if (Target.tag == "Wolf" || Target.tag == "Bear")
      {
        return AnimalState.Fleeing;
      }
      MovementController.RunToTarget(Target.transform.position);
      return Type();
    }

    public override AnimalState Type()
    {
      return AnimalState.RunningTowardsFood;
    }

    public override void OnTriggerEnter(Collider other)
    {
      var tag = other.gameObject.tag;
      if (MovementController.IsReachable(other.gameObject.transform.position))
      {
        if (tag == "Water")
        {
          MemoryController.SaveToMemory(other.gameObject);
          return;
        }

        if (tag == "Predator")
        {
          Target = other.gameObject;
          return;
        }

        if (tag == "Food")
        {
          //TODO: Check if the new food is closer than the current target
          MemoryController.SaveToMemory(other.gameObject); 
          return;
        }
      }
    }
  }
}
