namespace Ecosystem.AnimalBehaviour.Predators.Bear
{
  public sealed class BearStateController : AbstractPredatorStateController
  {
    protected override void Initialize()
    {
      base.Initialize();
      LookingForFood = PredatorStateFactory.CreateBearLookingForFood(Data);
      ChasingPrey = PredatorStateFactory.CreateBearChasingFood(Data);
      LookingForMate = PredatorStateFactory.CreateBearLookingForMate(Data);
    }
  }
}