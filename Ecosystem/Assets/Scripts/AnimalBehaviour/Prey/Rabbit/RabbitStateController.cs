using Ecosystem.Consumers;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Rabbit
{
  public sealed class RabbitStateController : AbstractPreyStateController
  {
    [SerializeField] private RabbitConsumer rabbitConsumer;

    protected override void Initialize()
    {
      base.Initialize();
      LookingForFood = PreyStateFactory.CreateRabbitLookingForFood(Data);
      RunningTowardsFood = PreyStateFactory.CreateRabbitRunningTowardsFood(Data);
      LookingForMate = PreyStateFactory.CreateRabbitLookingForMate(Data);
      Eating = PreyStateFactory.CreateRabbitEatingState(Data);
    }
  }
}