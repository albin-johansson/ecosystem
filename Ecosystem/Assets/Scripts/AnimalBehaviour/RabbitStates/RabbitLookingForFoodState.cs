using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitLookingForFoodState : AbstractAnimalState
  {
    public RabbitLookingForFoodState(RabbitStateData data)
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

        if(Target.tag == "Food")
        {
          return AnimalState.RunningTowardsFood;
        }
      }
      
      if(Consumer.Hunger < WaterConsumer.Thirst && WaterConsumer.IsThirsty())
      {
        return AnimalState.LookingForWater;
      }
      MovementController.UpdateWander();
      return Type();
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

        if (tag == "Wolf" || tag == "Bear")
        {
          Target = other.gameObject;
          return;
        }

        if (tag == "Food")
        {
          MemoryController.SaveToMemory(other.gameObject);
          Target = other.gameObject;
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
