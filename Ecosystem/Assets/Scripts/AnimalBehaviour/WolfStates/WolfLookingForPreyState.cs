using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public class WolfLookingForPreyState : AbstractAnimalState
  {
    public WolfLookingForPreyState(WolfStateData data)
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
    }

    public override AnimalState Tick()
    {
      if (Target != null)
      {
        return AnimalState.ChasingPrey;
      }

      if (WaterConsumer.Thirst > Consumer.Hunger && WaterConsumer.IsThirsty()) //If we are more thirsty then hungry: Look for water
      {
        return AnimalState.LookingForWater;
      }

      MovementController.UpdateWander();
      return this.Type();
    }

    override public void OnTriggerEnter(Collider other)
    {
      if (MovementController.IsReachable(other.gameObject.transform.position))
      {
        if (other.gameObject.CompareTag("Water"))
        {
          MemoryController.SaveToMemory(other.gameObject);
          return;
        }

        if (other.gameObject.CompareTag("Rabbit") || other.gameObject.CompareTag("Deer"))
        {
          Target = other.gameObject;
        }
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForPrey;
    }
  }
}
