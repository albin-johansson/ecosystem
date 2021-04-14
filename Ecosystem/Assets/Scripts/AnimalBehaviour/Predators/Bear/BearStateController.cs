using Ecosystem.Consumers;
using UnityEngine;


namespace Ecosystem.AnimalBehaviour.Predators.Bear
{
  public sealed class BearStateController : AbstractPredatorStateController
  {
    [SerializeField] private BearConsumer bearConsumer;


    protected override void Initialize()
    {
      base.Initialize();
      RunningTowardsFood = PredatorStateFactory.CreateBearRunningTowardsFoodState(Data);
      Eating = PredatorStateFactory.CreateBearEatingState(Data);
      LookingForFood = PredatorStateFactory.CreateBearLookingForFood(Data);
      ChasingPrey = PredatorStateFactory.CreateBearChasingFood(Data);
      LookingForMate = PredatorStateFactory.CreateBearLookingForMate(Data);
    }
  }
}