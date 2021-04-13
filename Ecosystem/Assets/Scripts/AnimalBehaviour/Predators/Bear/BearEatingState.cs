using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Bear
{
  internal sealed class BearEatingState : AbstractAnimalState
  {
    public BearEatingState(StateData data)
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
      AnimationController.IdleAnimation();
      MovementController.StandStill(true);
    }

    public override AnimalState Tick()
    {
      if (Consumer.EatingFromGameObject)
      {
        return Type();
      }
      else
      {
        return base.Tick();
      }
    }

    public override AnimalState Type()
    {
      return AnimalState.Eating;
    }
  }
}