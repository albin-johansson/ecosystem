using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Bear
{
  internal sealed class BearLookingForFoodState : AbstractAnimalState
  {
    internal BearLookingForFoodState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = GetClosestInVision(Layers.PreyMask | Layers.MeatMask |Layers.StaticFoodMask);
      MovementController.StartWander();
      AnimationController.EnterMoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        if (Tags.IsMeat(Target) || Tags.IsStaticFood(Target))
        {
          return AnimalState.GoingToFood;
        }

        if (Tags.IsPrey(Target))
        {
          return AnimalState.ChasingPrey;
        }
      }

      MovementController.UpdateWander();
      return base.Tick();
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsWater(otherObject))
      {
        MemoryController.SaveToMemory(otherObject);
      }
      else if (Tags.IsPrey(otherObject) || Tags.IsMeat(otherObject) || Tags.IsStaticFood(otherObject))
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