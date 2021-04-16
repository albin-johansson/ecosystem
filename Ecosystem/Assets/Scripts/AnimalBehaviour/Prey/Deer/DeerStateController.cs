namespace Ecosystem.AnimalBehaviour.Prey.Deer
{
  public sealed class DeerStateController : AbstractPreyStateController
  {
    protected override void Initialize()
    {
      base.Initialize();
      LookingForFood = PreyStateFactory.CreateDeerLookingForFood(Data);
      RunningTowardsFood = PreyStateFactory.CreateDeerRunningTowardsFood(Data);
      LookingForMate = PreyStateFactory.CreateDeerLookingForMate(Data);
    }
  }
}