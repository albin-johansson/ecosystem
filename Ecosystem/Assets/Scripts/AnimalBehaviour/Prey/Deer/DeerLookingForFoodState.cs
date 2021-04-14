using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Deer
{
  internal sealed class DeerLookingForFoodState : AbstractAnimalState
  {
    private double _time;
    private double _limit = 10;

    internal DeerLookingForFoodState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = GetClosestInVision(Layers.FoodMask);
      MovementController.StartWander();
      _time = 0;
      _limit = Random.Range(5, 15);
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
      }

      if (Consumer.Hunger < WaterConsumer.Thirst && WaterConsumer.IsThirsty())
      {
        return AnimalState.LookingForWater;
      }

      if (_time > _limit)
      {
        return AnimalState.Eating;
      }

      _time += Time.deltaTime;

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