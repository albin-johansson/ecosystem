using Ecosystem.Consumers;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Rabbit
{
  public sealed class RabbitStateController : AbstractPreyStateController
  {
    [SerializeField] private RabbitConsumer rabbitConsumer;

    public override void Start()
    {
      Consumer = rabbitConsumer;
      base.Start();
      LookingForFood = PreyStateFactory.CreateRabbitLookingForFood(Data);
      RunningTowardsFood = PreyStateFactory.CreateRabbitRunningTowardsFood(Data);
      LookingForMate = PreyStateFactory.CreateRabbitLookingForMate(Data);
      Eating = PreyStateFactory.CreateRabbitEatingState(Data);
    }
  }
}