using Ecosystem.Consumers;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Bear
{
  public sealed class BearStateController : AbstractPredatorStateController
  {
    [SerializeField] private BearConsumer bearConsumer;

    public override void Start()
    {
      Consumer = bearConsumer;
      base.Start();
      RunningTowardsFood = PredatorStateFactory.CreateBearRunningTowardsFoodState(Data);
      Eating = PredatorStateFactory.CreateBearEatingState(Data);
      LookingForFood = PredatorStateFactory.CreateBearLookingForFood(Data);
      ChasingPrey = PredatorStateFactory.CreateBearChasingFood(Data);
      LookingForMate = PredatorStateFactory.CreateBearLookingForMate(Data);
    }
  }
}