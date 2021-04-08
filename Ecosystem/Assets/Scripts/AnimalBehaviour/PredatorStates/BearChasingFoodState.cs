using Ecosystem.Util;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.PredatorStates
{
  internal sealed class BearChasingFoodState : AbstractAnimalState
  {
    public BearChasingFoodState(StateData data)
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
      MovementController.StartHunting(Target.transform.position);
      AnimationController.MoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (!Target || !Consumer.IsHungry())
      {
        return base.Tick();
      }
      else if (!Target.activeSelf || !MovementController.IsTargetInRange(Target.transform.position))
      {
        Target = GetClosestInVision(Layers.PreyMask);
        return Type();
      }
      else if (Consumer.IsAttacking)
      {
        return AnimalState.Attacking;
      }
      else
      {
        MovementController.UpdateHunting(Target.transform.position);
      }

      return Type();
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsPrey(otherObject))
      {
        Target = SelectCloser(Target, otherObject);
      }
    }

    public override void OnTriggerExit(Collider other)
    {
      if (other.gameObject == Target)
      {
        Target = GetClosestInVision(Layers.PreyMask | Layers.MeatMask);
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.ChasingPrey;
    }
  }
}