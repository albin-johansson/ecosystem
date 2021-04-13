using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Bear
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
      MovementController.SetDestination(Target.transform.position);
      AnimationController.EnterMoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (!Target || !Consumer.IsHungry())
      {
        return base.Tick();
      }
      else if (!Target.activeSelf || !MovementController.IsWithinSphere(Target.transform.position))
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
        MovementController.SetDestination(Target.transform.position);
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
        Target = GetClosestInVision(Layers.PreyMask);
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.ChasingPrey;
    }
  }
}