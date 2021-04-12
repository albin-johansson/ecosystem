using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators
{
  internal sealed class PredatorIdleState : AbstractAnimalState
  {
    public PredatorIdleState(StateData data)
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
      Target = null;
      AnimationController.IdleAnimation();
    }

    public override AnimalState Type()
    {
      return AnimalState.Idle;
    }
  }
}