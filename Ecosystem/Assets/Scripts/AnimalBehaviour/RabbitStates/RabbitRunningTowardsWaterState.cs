using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitRunningTowardsWaterState : AbstractAnimalState
  {
    public RabbitRunningTowardsWaterState(RabbitStateData data)
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
      
      if(Target.tag == "Wolf" || Target.tag == "Bear")
      {
        return AnimalState.Fleeing;
      }

      if(WaterConsumer.CanDrink)
      {
        return AnimalState.Drinking;
      }
      MovementController.RunToTarget(Target.transform.position);
      return Type();
    }

    public override AnimalState Type()
    {
      return AnimalState.RunningTowardsWater;
    }

    public override void OnTriggerEnter(Collider other)
      {
      var tag = other.gameObject.tag;
      if (MovementController.IsReachable(other.gameObject.transform.position))
      {
        if (tag == "Water")
        {
          //TODO: Check if new source is closer
          MemoryController.SaveToMemory(other.gameObject);
          return;
        }

        if (tag == "Wolf" || tag == "Bear")
        {
          Target = other.gameObject;
          return;
        }

        if (tag == "Food")
        {
          MemoryController.SaveToMemory(other.gameObject); 
          return;
        }
      }
    }
  }
}
