using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators
{
  internal sealed class PredatorRunningTowardsWaterState : AbstractAnimalState
  {
    internal PredatorRunningTowardsWaterState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = target;
      MovementController.SetDestinationIfValid(Target.transform.position);
      AnimationController.EnterMoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (!Target || !WaterConsumer.IsThirsty())
      {
        return base.Tick();
      }
      else if (WaterConsumer.CanDrink)
      {
        return AnimalState.Drinking;
      }
      else
      {
        MovementController.SetDestinationIfValid(Target.transform.position);
        return Type();
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.RunningTowardsWater;
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsWater(otherObject))
      {
        MemoryController.SaveToMemory(otherObject);
        Target = SelectCloser(otherObject, Target);
      }
    }
  }
}