using Ecosystem.Consumers;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Wolf
{
  public sealed class WolfStateController : AbstractPredatorStateController
  {
    [SerializeField] private WolfConsumer wolfConsumer;

    public override void Start()
    {
      Consumer = wolfConsumer;
      base.Start();
      RunningTowardsFood = PredatorStateFactory.CreateWolfRunningTowardsFoodState(Data);
      LookingForFood = PredatorStateFactory.CreateWolfLookingForPrey(Data);
      ChasingPrey = PredatorStateFactory.CreateWolfChasingPrey(Data);
      LookingForMate = PredatorStateFactory.CreateWolfLookingForMate(Data);
    }
  }
}