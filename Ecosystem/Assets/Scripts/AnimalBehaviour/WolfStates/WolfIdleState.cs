using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  internal sealed class WolfIdleState : AbstractAnimalState
  {
    public WolfIdleState(WolfStateData data)
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
      MovementController.StartWander();
    }

    public override AnimalState Tick()
    {
      MovementController.UpdateWander();
      return base.Tick();
    }

    public override AnimalState Type()
    {
      return AnimalState.Idle;
    }
  }
}
