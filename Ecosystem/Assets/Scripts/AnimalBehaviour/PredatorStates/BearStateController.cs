using UnityEngine;

namespace Ecosystem.AnimalBehaviour.PredatorStates
{
  public sealed class BearStateController : AbstractPredatorStateController
  {
    [SerializeField] private PreyConsumer preyConsumer;

    public override void Start()
    {
      Consumer = preyConsumer;
      base.Start();
      LookingForFood = PredatorStateFactory.CreateBearLookingForFood(Data);
      ChasingPrey = PredatorStateFactory.CreateBearChasingFood(Data);
      LookingForMate = PredatorStateFactory.CreateBearLookingForMate(Data);
    }
  }
}