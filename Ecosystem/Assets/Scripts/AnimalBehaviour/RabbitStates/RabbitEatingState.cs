using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  internal sealed class RabbitEatingState : AbstractAnimalState
  {
    public RabbitEatingState(RabbitStateData data)
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
      AnimationController.IdleAnimation();
      MovementController.StandStill(true);
      // TODO Check memory
    }

    public override AnimalState Tick()
    {
      if (Tags.IsPredator(Target))
      {
        return AnimalState.Fleeing;
      }

      if (Consumer.EatingFromGameObject)
      {
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
        if (Tags.IsPredator(otherObject))
        {
          Target = otherObject;
        }
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.Eating;
    }
  }
}