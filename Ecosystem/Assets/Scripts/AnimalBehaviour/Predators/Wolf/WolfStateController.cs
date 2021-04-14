namespace Ecosystem.AnimalBehaviour.Predators.Wolf
{
  public sealed class WolfStateController : AbstractPredatorStateController
  {
    protected override void Initialize()
    {
      base.Initialize();
      LookingForFood = PredatorStateFactory.CreateWolfLookingForFood(Data);
      ChasingPrey = PredatorStateFactory.CreateWolfChasingPrey(Data);
      LookingForMate = PredatorStateFactory.CreateWolfLookingForMate(Data);
    }
  }
}