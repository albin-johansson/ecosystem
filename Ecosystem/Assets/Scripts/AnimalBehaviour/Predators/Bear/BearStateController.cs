using Ecosystem.Consumer;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Bear
{
  public sealed class BearStateController : AbstractPredatorStateController
  {
    [SerializeField] private BearConsumer bearConsumer;

    protected override void Initialize()
    {
      Consumer = bearConsumer;
      base.Initialize();
      RunningTowardsFood = PredatorStateFactory.CreateBearRunningTowardsFood(Data);
      LookingForFood = PredatorStateFactory.CreateBearLookingForFood(Data);
      ChasingPrey = PredatorStateFactory.CreateBearChasingPrey(Data);
      LookingForMate = PredatorStateFactory.CreateBearLookingForMate(Data);
      Eating = PredatorStateFactory.CreateBearEating(Data);
    }
  }
}