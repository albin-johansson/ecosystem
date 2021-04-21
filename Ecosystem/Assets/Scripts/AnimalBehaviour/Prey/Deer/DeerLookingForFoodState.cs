using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Deer
{
  internal sealed class DeerLookingForFoodState : AbstractAnimalState
  {
    private float _time;
    private float _limit;

    internal DeerLookingForFoodState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = null;
      MovementController.StartWander();
      AnimationController.EnterMoveAnimation();
      _time = 0;
      _limit = Random.Range(1, 10);
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

    public override void OnSphereEnter(Collider other)
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