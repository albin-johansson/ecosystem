﻿using Ecosystem.Components;
using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Deer
{
  internal sealed class DeerEatingState : AbstractAnimalState
  {
    public DeerEatingState(StateData data)
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
      AnimationController.EatingAnimation();
      MovementController.StandStill(true);
      Consumer.IsConsuming = true;
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        if (Tags.IsPredator(Target))
        {
          return AnimalState.Fleeing;
        } 
      }

      if (Consumer.Hunger < WaterConsumer.Thirst && WaterConsumer.IsThirsty())
      {
        return AnimalState.LookingForWater;
      }

      if (Consumer.IsConsuming)
      {
        return Type();
      }
      else
      {
        return base.Tick();
      }
    }

    public override GameObject End()
    {
      Consumer.IsConsuming = false;
      return base.End();
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsPredator(otherObject))
      {
        Target = otherObject;
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.Eating;
    }
  }
}