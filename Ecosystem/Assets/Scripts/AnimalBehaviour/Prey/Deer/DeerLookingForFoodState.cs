using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Deer
{
  internal sealed class DeerLookingForFoodState : AbstractAnimalState
  {
    internal DeerLookingForFoodState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = GetClosestInVision(Layers.FoodMask);
      MovementController.StartWander();
      AnimationController.EnterMoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        if (Tags.IsPredator(Target))
        {
          return AnimalState.Fleeing;
        }
        else if (Tags.IsFood(Target))
        {
          return AnimalState.RunningTowardsFood;
        }
      }

      if (Consumer.Hunger < WaterConsumer.Thirst && WaterConsumer.IsThirsty())
      {
        return AnimalState.LookingForWater;
      }

      MovementController.UpdateWander();

      return Type();
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsWater(otherObject))
      {
        MemoryController.SaveToMemory(otherObject);
      }
      else if (Tags.IsFood(otherObject))
      {
        Target = otherObject;
      }
      else if (Tags.IsPredator(otherObject))
      {
        Target = otherObject;
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForFood;
    }
  }
}