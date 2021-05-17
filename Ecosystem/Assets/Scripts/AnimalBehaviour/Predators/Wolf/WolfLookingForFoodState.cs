using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Wolf
{
  internal sealed class WolfLookingForFoodState : AbstractAnimalState
  {
    internal WolfLookingForFoodState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = GetClosestInVision(Layers.PreyMask | Layers.MeatMask);
      AnimationController.EnterMoveAnimation();
      MovementController.StartWander();
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        if (Tags.IsMeat(Target))
        {
          return AnimalState.RunningTowardsFood;
        }
        else if (Tags.IsPrey(Target))
        {
          return AnimalState.ChasingPrey;
        }
      }

      MovementController.UpdateWander();
      return base.Tick();
    }

    public override void OnSphereEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsWater(otherObject))
      {
        MemoryController.SaveToMemory(otherObject);
      }
      else if (Tags.IsPrey(otherObject) || Tags.IsMeat(otherObject))
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