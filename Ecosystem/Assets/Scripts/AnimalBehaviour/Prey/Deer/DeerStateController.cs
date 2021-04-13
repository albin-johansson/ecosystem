using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Deer
{
  public sealed class DeerStateController : AbstractPreyStateController
  {
    [SerializeField] private GrassConsumer grassConsumer;

    public override void Start()
    {
      Consumer = grassConsumer;
      base.Start();
      LookingForFood = PreyStateFactory.CreateDeerLookingForFood(Data);
      RunningTowardsFood = PreyStateFactory.CreateDeerRunningTowardsFood(Data);
      LookingForMate = PreyStateFactory.CreateDeerLookingForMate(Data);
      Eating = PreyStateFactory.CreateDeerEatingState(Data);
    }
  }
}