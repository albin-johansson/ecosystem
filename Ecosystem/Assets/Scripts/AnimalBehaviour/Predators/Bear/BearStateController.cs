using Ecosystem.Consumer;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Bear
{
  public sealed class BearStateController : AbstractPredatorStateController
  {
    [SerializeField] private BearConsumer bearConsumer;
    protected override void Initialize()
    {
      Data.Consumer = bearConsumer;
      base.Initialize();
      LookingForFood = PredatorStateFactory.CreateBearLookingForFood(Data);
      ChasingPrey = PredatorStateFactory.CreateBearChasingFood(Data);
      LookingForMate = PredatorStateFactory.CreateBearLookingForMate(Data);
    }
  }
}