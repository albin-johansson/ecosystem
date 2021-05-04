using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators
{
  internal sealed class PredatorIdleState : AbstractAnimalState
  {
    private float _elapsedTime;
    private float _standStillThreshold = 3f;
    private bool _standStill = true;

    internal PredatorIdleState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Target = null;
      MovementController.ClearNavigationTarget();
      AnimationController.EnterIdleAnimation();
    }

    public override AnimalState Tick()
    {
      if (_standStill)
      {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _standStillThreshold)
        {
          AnimationController.EnterMoveAnimation();
          MovementController.StartWander();
          _elapsedTime = 0;
          _standStill = false;
        }
      }
      else
      {
        MovementController.UpdateWander();
      }

      return base.Tick();
    }

    public override void OnSphereEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsWater(otherObject))
      {
        MemoryController.SaveToMemory(otherObject);
      }
    }

    public override GameObject End()
    {
      _standStill = true;
      return base.End();
    }

    public override AnimalState Type()
    {
      return AnimalState.Idle;
    }
  }
}