using Ecosystem.Consumer;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Rabbit
{
  public sealed class RabbitStateController : AbstractPreyStateController
  {
    [SerializeField] private RabbitConsumer rabbitConsumer;

    protected override void Initialize()
    {
      Consumer = rabbitConsumer;
      base.Initialize();
      Eating = PreyStateFactory.CreateRabbitEating(Data);
      LookingForFood = PreyStateFactory.CreateRabbitLookingForFood(Data);
      RunningTowardsFood = PreyStateFactory.CreateRabbitRunningTowardsFood(Data);
      LookingForMate = PreyStateFactory.CreateRabbitLookingForMate(Data);
    }
  }
}