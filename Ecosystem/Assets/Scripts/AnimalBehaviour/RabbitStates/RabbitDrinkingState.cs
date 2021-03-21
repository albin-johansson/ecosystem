using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitDrinkingState : AbstractAnimalState
  {
    public RabbitDrinkingState(RabbitStateData data)
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
      movementController.StandStill(true);
      // TODO: Add animationController.DrinkAnimation();
      //TODO Check memory
    }

    override public AnimalState Tick()
    {
      if (_target.CompareTag("Predator"))
      {
        return AnimalState.Fleeing;
      }

      if (waterConsumer.IsDrinking)
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
          memoryController.SaveToMemory(other.gameObject); 
          return;
        }
      }
    }
  }
}
