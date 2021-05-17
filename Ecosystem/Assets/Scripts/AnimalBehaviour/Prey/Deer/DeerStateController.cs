using Ecosystem.Consumer;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Deer
{
  public sealed class DeerStateController : AbstractPreyStateController
  {
    [SerializeField] private DeerConsumer deerConsumer;

    protected override void Initialize()
    {
      Consumer = deerConsumer;
      base.Initialize();
      Eating = PreyStateFactory.CreateDeerEating(Data);
      LookingForFood = PreyStateFactory.CreateDeerLookingForFood(Data);
      RunningTowardsFood = PreyStateFactory.CreateDeerRunningTowardsFood(Data);
      LookingForMate = PreyStateFactory.CreateDeerLookingForMate(Data);
    }
  }
}