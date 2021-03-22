using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitLookingForWaterState : AbstractAnimalState
  {
    public RabbitLookingForWaterState(RabbitStateData data)
    {
      Consumer = data.Consumer;
      WaterConsumer = data.WaterConsumer;
      MovementController = data.MovementController;
      AnimationController = data.AnimationController;
      MemoryController = data.MemoryController;
    }
    
    public override void Begin(GameObject target)
    {
      Target = null;
      MovementController.StartWander();
      AnimationController.MoveAnimation();
      //TODO Check memory
    }

    public override AnimalState Tick()
    { 
      if(Target != null)
      {
        if(Target.tag == "Wolf" || Target.tag == "Bear")
        {
          return AnimalState.Fleeing;
        }

        if(Target.tag == "Water")
        {
          return AnimalState.RunningTowardsWater;
        }
      }
      
      if(Consumer.Hunger > WaterConsumer.Thirst && Consumer.IsHungry())
      {
        return AnimalState.LookingForFood;
      }
      MovementController.UpdateWander();
      return Type();
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForWater;
    }

    public override void OnTriggerEnter(Collider other)
    {
      var tag = other.gameObject.tag;
      if (MovementController.IsReachable(other.gameObject.transform.position))
      {
        if (tag == "Water")
        {
          MemoryController.SaveToMemory(other.gameObject);
          Target = other.gameObject;
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
