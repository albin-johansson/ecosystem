using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Wolf
{
  internal sealed class WolfLookingForPreyState : AbstractAnimalState
  {
    internal WolfLookingForPreyState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = GetClosestInVision(Layers.PreyMask | Layers.MeatMask);
      MovementController.StartWander();
      AnimationController.EnterMoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        if (Tags.IsMeat(Target))
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
      if (otherObject.CompareTag("Water"))
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