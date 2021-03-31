using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.PreyStates
{
  internal sealed class PreyIdleState : AbstractAnimalState
  {
    public PreyIdleState(StateData data)
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
      Target = null;
      AnimationController.IdleAnimation();
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        return AnimalState.Fleeing;
      }
      else
      {
        return base.Tick();
      }
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (otherObject.CompareTag("Water"))
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
      return AnimalState.Idle;
    }
  }
}