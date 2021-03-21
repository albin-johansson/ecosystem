using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public class WolfChasingPreyState : AbstractAnimalState
  {
    public WolfChasingPreyState(WolfStateData data)
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
      movementController.StartHunting(_target.transform.position);
      animationController.MoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (_target == null || !movementController.IsTargetInRange(_target.transform.position))
      {
        return base.Tick();
      }
      else
      {
        movementController.UpdateHunting(_target.transform.position);
      }

      if (waterConsumer.Thirst > consumer.Hunger && waterConsumer.IsThirsty()) //If we are more thirsty then hungry: Look for water
      {
        return AnimalState.LookingForWater;
      }


      return this.Type();
    }

    override public void OnTriggerExit(Collider other)
    {
      if (other.gameObject == _target)
      {
        _target = null;
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForWater;
    }
  }
}
