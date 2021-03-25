using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  internal sealed class RabbitFleeingState : AbstractAnimalState
  {
    public RabbitFleeingState(RabbitStateData data)
    {
      Consumer = data.Consumer;
      WaterConsumer = data.WaterConsumer;
      MovementController = data.MovementController;
      AnimationController = data.AnimationController;
      MemoryController = data.MemoryController;
      Reproducer = data.Reproducer;
    }

    public override void Begin(GameObject target)
    {
      Target = target;
      MovementController.StartFleeing(Target.transform.position);
      AnimationController.MoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        MovementController.UpdateFleeing(Target.transform.position);
        return Type();
      }
      else
      {
        return base.Tick();
      }
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (MovementController.IsReachable(otherObject.transform.position))
      {
        if (otherObject.CompareTag("Water"))
        {
          MemoryController.SaveToMemory(otherObject);
        }
        else if (Tags.IsPredator(otherObject))
        {
          //TODO: Add logic for tracking all Predators in collision
        }
      }
    }

    public override void OnTriggerExit(Collider other)
    {
      //TODO: Logic for looking for other predators already in the collider and set the closest one to current target.
      if (other.gameObject == Target)
      {
        Target = null;
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.Fleeing;
    }
  }
}