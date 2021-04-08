using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.PreyStates
{
  internal sealed class RabbitRunningTowardsFoodState : AbstractAnimalState
  {
    public RabbitRunningTowardsFoodState(StateData data)
    {
      StaminaController = data.StaminaController;
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
      MovementController.RunToTarget(Target.transform.position);
      AnimationController.MoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (!Target || !Consumer.IsHungry() || !Target.activeSelf)
      {
        return base.Tick();
      }
      else if (Tags.IsPredator(Target))
      {
        return AnimalState.Fleeing;
      }
      else if (Consumer.EatingFromGameObject)
      {
        return AnimalState.Eating;
      }
      else
      {
        MovementController.RunToTarget(Target.transform.position);
        return Type();
      }
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (otherObject.CompareTag("Food"))
      {
        Target = SelectCloser(otherObject, Target);
      }
      else if (otherObject.CompareTag("Water"))
      {
        MemoryController.SaveToMemory(otherObject);
      }
      else if (Tags.IsPredator(otherObject))
      {
        Target = otherObject;
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.RunningTowardsFood;
    }
  }
}