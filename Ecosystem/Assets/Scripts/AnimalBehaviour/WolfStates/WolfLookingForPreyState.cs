using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public class WolfLookingForPreyState : AbstractAnimalState
  {
    public WolfLookingForPreyState(WolfStateData data)
    {
      consumer = data.consumer;
      waterConsumer = data.waterConsumer;
      movementController = data.movementController;
      animationController = data.animationController;
      memoryController = data.memoryController;
    }

    public override void Begin(GameObject target)
    {
      _target = null;
      movementController.StartWander();
      animationController.MoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (_target != null)
      {
        return AnimalState.ChasingPrey;
      }

      if (waterConsumer.Thirst > consumer.Hunger && waterConsumer.IsThirsty()) //If we are more thirsty then hungry: Look for water
      {
        return AnimalState.LookingForWater;
      }

      movementController.UpdateWander();
      return this.Type();
    }

    override public void OnTriggerEnter(Collider other)
    {
      if (movementController.IsReachable(other.gameObject.transform.position))
      {
        if (other.gameObject.CompareTag("Water"))
        {
          memoryController.SaveToMemory(other.gameObject);
          return;
        }

        if (other.gameObject.CompareTag("Rabbit") || other.gameObject.CompareTag("Deer"))
        {
          _target = other.gameObject;
        }
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForPrey;
    }
  }
}
