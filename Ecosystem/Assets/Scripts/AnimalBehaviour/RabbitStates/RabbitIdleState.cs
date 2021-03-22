using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitIdleState : AbstractAnimalState
  {
    public RabbitIdleState(RabbitStateData data)
    {
      Consumer = data.consumer;
      WaterConsumer = data.waterConsumer;
      MovementController = data.movementController;
      AnimationController = data.animationController;
      MemoryController = data.memoryController;
    }
    
    public override void Begin(GameObject target)
    {
      Target = null;
      MovementController.StartWander();
      AnimationController.MoveAnimation();
      //TODO Check memory
    }

    override public AnimalState Tick()
    {
      if (Target != null)
      {
        return AnimalState.Fleeing;
      }

      if (Consumer.Hunger >= WaterConsumer.Thirst && Consumer.IsHungry())
      {
        return AnimalState.LookingForFood;
      }

      if (WaterConsumer.Thirst > Consumer.Hunger && WaterConsumer.IsThirsty())
      {
        return AnimalState.LookingForWater;
      }

      return Type();
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
          Target = other.gameObject;
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
