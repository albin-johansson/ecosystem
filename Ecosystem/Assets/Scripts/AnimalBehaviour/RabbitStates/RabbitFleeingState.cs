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
      Genome = data.Genome;
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
        if (Target.activeSelf)
        {
          MovementController.UpdateFleeing(Target.transform.position);
          return Type();
        }
        else
        {
          Target = GetClosestInVision(Layers.PredatorLayer);
          if (Target)
          {
            MovementController.UpdateFleeing(Target.transform.position);
            return Type();
          }
        }
      }
      return base.Tick();
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (otherObject.CompareTag("Water"))
      {
        MemoryController.SaveToMemory(otherObject);
      }
    }

    public override void OnTriggerExit(Collider other)
    {
      if (other.gameObject == Target)
      {
        Target = GetClosestInVision(Layers.PredatorLayer);
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.Fleeing;
    }
  }
}
