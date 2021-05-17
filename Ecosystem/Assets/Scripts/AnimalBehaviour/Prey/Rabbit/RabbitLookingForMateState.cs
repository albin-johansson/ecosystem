using Ecosystem.Util;
using UnityEditor;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Rabbit
{
  public class RabbitLookingForMateState : AbstractAnimalState
  {
    private float _timer;

    internal RabbitLookingForMateState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Reproducer.IsWilling = true;
      AnimationController.EnterMoveAnimation();
      Target = GetClosestMateInVision(Layers.RabbitMask);
      MovementController.StartWander();
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForMate;
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        _timer = 0;
        if (Tags.IsPredator(Target))
        {
          return AnimalState.Fleeing;
        }

        if (!Target.activeInHierarchy || !MovementController.IsWithinSphere(Target.transform.position))
        {
          Target = null;
        }
        else if (Reproducer.CompatibleAsParents(Target))
        {
          MovementController.SetDestination(Target.transform.position);
          return Type();
        }
        else
        {
          Target = null;
          MovementController.StartWander();
        }
      }
      else
      {
        _timer += Time.deltaTime;
        if (_timer > 1)
        {
          Target = GetClosestMateInVision(Layers.RabbitMask);
          _timer = 0;
        }
        MovementController.UpdateWander();
      }

      return base.Tick();
    }

    public override GameObject End()
    {
      Reproducer.IsWilling = false;
      return base.End();
    }

    public override void OnSphereEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (Tags.IsWater(otherObject))
      {
        MemoryController.SaveToMemory(otherObject);
      }
      else if (Reproducer.CompatibleAsParents(otherObject) || Tags.IsPredator(otherObject))
      {
        Target = otherObject;
      }
    }

    public override void OnSphereExit(Collider other)
    {
      if (other.gameObject == Target)
      {
        Target = GetClosestMateInVision(Layers.RabbitMask);
      }
    }
  }
}
