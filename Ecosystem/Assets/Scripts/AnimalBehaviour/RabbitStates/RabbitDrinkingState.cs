using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitDrinkingState : AbstractAnimalState
  {
    public RabbitDrinkingState(RabbitStateData data)
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
      MovementController.StandStill(true);
      // TODO: Add animationController.DrinkAnimation();
      //TODO Check memory
    }

    override public AnimalState Tick()
    {
      if (Target.CompareTag("Wolf") || Target.CompareTag("Bear"))
      {
        return AnimalState.Fleeing;
      }

      if (WaterConsumer.IsDrinking)
      {
        return this.Type();
      }
      return base.Tick();
    }

    public override AnimalState Type()
    {
      return AnimalState.Drinking;
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
          return;
        }
      }
    }
  }
}
