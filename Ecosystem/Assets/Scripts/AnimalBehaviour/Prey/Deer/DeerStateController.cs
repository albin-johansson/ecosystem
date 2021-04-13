using Ecosystem.Consumers;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Deer
{
  public sealed class DeerStateController : AbstractPreyStateController
  {
    [SerializeField] private DeerConsumer deerConsumer;

    public override void Start()
    {
      Consumer = deerConsumer;
      base.Start();
      LookingForFood = PreyStateFactory.CreateDeerLookingForFood(Data);
      RunningTowardsFood = PreyStateFactory.CreateDeerRunningTowardsFood(Data);
      LookingForMate = PreyStateFactory.CreateDeerLookingForMate(Data);
      Eating = PreyStateFactory.CreateDeerEatingState(Data);
    }
  }
}