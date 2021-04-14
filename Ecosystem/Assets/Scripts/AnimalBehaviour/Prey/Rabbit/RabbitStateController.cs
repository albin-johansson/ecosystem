namespace Ecosystem.AnimalBehaviour.Prey.Rabbit
{
  public sealed class RabbitStateController : AbstractPreyStateController
  {
    protected override void Initialize()
    {
      base.Initialize();
      LookingForFood = PreyStateFactory.CreateRabbitLookingForFood(Data);
      RunningTowardsFood = PreyStateFactory.CreateRabbitRunningTowardsFood(Data);
      LookingForMate = PreyStateFactory.CreateRabbitLookingForMate(Data);
    }
  }
}