using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  internal sealed class RabbitFleeingState : AbstractAnimalState
  {
    private GameObject _lastPredatorSeen;

    public RabbitFleeingState(RabbitStateData data)
    {
      StaminaController = data.StaminaController;
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
      _lastPredatorSeen = target;
      MovementController.StartFleeing(Target.transform.position);
    }

    public override AnimalState Tick()
    {
      if (Target && Target.activeSelf)
      {
        if (StaminaController.CanRun())
        {
          StaminaController.IsRunning = true;
          AnimationController.RunAnimation();
        }
        else
        {
          AnimationController.MoveAnimation();
        }

        MovementController.UpdateFleeing(Target.transform.position);
        return Type();
      }
      else
      {
        return base.Tick();
      }
    }

    public override GameObject End()
    {
      StaminaController.IsRunning = false;
      return base.End();
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (MovementController.IsReachable(otherObject.transform.position))
      {
        if (otherObject.CompareTag("Water"))
        {
          MemoryController.SaveToMemory(otherObject);
        }
        else if (Tags.IsPredator(otherObject))
        {
          _lastPredatorSeen = otherObject;
        }
      }
    }

    public override void OnTriggerExit(Collider other)
    {
      if (other.gameObject == Target)
      {
        if (other.gameObject == _lastPredatorSeen)
        {
          Target = null;
        }
        else
        {
          Target = _lastPredatorSeen;
        }
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.Fleeing;
    }
  }
}