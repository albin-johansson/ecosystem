using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Rabbit
{
  public sealed class RabbitStateController : AbstractPreyStateController
  {
    [SerializeField] private FoodConsumer foodConsumer;

    public override void Start()
    {
      Consumer = foodConsumer;
      base.Start();
      LookingForFood = PreyStateFactory.CreateRabbitLookingForFood(Data);
      RunningTowardsFood = PreyStateFactory.CreateRabbitRunningTowardsFood(Data);
      LookingForMate = PreyStateFactory.CreateRabbitLookingForMate(Data);
      Eating = PreyStateFactory.CreateRabbitEatingState(Data);
    }
  }
}