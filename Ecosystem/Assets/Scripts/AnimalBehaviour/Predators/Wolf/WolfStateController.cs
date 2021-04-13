using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators.Wolf
{
  public sealed class WolfStateController : AbstractPredatorStateController
  {
    [SerializeField] private PreyConsumer preyConsumer;

    public override void Start()
    {
      Consumer = preyConsumer;
      base.Start();
      LookingForFood = PredatorStateFactory.CreateWolfLookingForPrey(Data);
      ChasingPrey = PredatorStateFactory.CreateWolfChasingPrey(Data);
      LookingForMate = PredatorStateFactory.CreateWolfLookingForMate(Data);
    }
  }
}