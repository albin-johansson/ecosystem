using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Wolf
{
  public class WolfLookingForMateState : AbstractAnimalState
  {
    private float _timer;

    internal WolfLookingForMateState(StateData data) : base(data)
    {
    }

    public override void Begin(GameObject target)
    {
      Reproducer.IsWilling = true;
      AnimationController.EnterMoveAnimation();
      Target = GetClosestMateInVision(Layers.WolfMask);
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
          Target = GetClosestMateInVision(Layers.WolfMask);
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
      else if (Reproducer.CompatibleAsParents(otherObject))
      {
        Target = otherObject;
      }
    }

    public override void OnSphereExit(Collider other)
    {
      if (other.gameObject == Target)
      {
        Target = GetClosestMateInVision(Layers.WolfMask);
      }
    }
  }
}
