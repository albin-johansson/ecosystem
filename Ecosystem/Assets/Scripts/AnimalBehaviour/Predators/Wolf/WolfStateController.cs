using Ecosystem.Consumer;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Wolf
{
  public sealed class WolfStateController : AbstractPredatorStateController
  {
    [SerializeField] private WolfConsumer wolfConsumer;

    protected override void Initialize()
    {
      consumer = wolfConsumer;
      base.Initialize();
      RunningTowardsFood = PredatorStateFactory.CreateWolfRunningTowardsFood(Data);
      LookingForFood = PredatorStateFactory.CreateWolfLookingForFood(Data);
      ChasingPrey = PredatorStateFactory.CreateWolfChasingPrey(Data);
      LookingForMate = PredatorStateFactory.CreateWolfLookingForMate(Data);
    }
  }
}