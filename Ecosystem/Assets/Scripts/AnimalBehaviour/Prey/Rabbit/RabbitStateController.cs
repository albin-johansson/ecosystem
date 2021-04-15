using Ecosystem.Consumer;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Rabbit
{
  public sealed class RabbitStateController : AbstractPreyStateController
  {
    [SerializeField] private RabbitConsumer rabbitConsumer;
    protected override void Initialize()
    {
      Data.Consumer = rabbitConsumer;
      base.Initialize();
      LookingForFood = PreyStateFactory.CreateRabbitLookingForFood(Data);
      RunningTowardsFood = PreyStateFactory.CreateRabbitRunningTowardsFood(Data);
      LookingForMate = PreyStateFactory.CreateRabbitLookingForMate(Data);
    }
  }
}