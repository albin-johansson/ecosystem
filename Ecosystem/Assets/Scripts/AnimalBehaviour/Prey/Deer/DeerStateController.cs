
using Ecosystem.Consumers;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Deer
{
  public sealed class DeerStateController : AbstractPreyStateController
  {

    [SerializeField] private DeerConsumer deerConsumer;
    
    protected override void Initialize()
    {
      base.Initialize();
      LookingForFood = PreyStateFactory.CreateDeerLookingForFood(Data);
      RunningTowardsFood = PreyStateFactory.CreateDeerRunningTowardsFood(Data);
      LookingForMate = PreyStateFactory.CreateDeerLookingForMate(Data);
      Eating = PreyStateFactory.CreateDeerEatingState(Data);
    }
  }
}