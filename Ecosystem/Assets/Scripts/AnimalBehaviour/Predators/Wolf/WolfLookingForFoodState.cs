using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Wolf
{
  internal sealed class WolfLookingForFoodState : AbstractAnimalState
  {
    public WolfLookingForFoodState(StateData data)
    {
      StaminaController = data.StaminaController;
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
      Target = GetClosestInVision(Layers.PreyMask);
      MovementController.StartWander();
      AnimationController.MoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        return AnimalState.ChasingPrey;
      }
      else
      {
        MovementController.UpdateWander();
        return base.Tick();
      }
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsWater(otherObject))
      {
        MemoryController.SaveToMemory(otherObject);
      }
      else if (Tags.IsPrey(otherObject))
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