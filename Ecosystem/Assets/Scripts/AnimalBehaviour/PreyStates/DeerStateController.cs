using UnityEngine;

namespace Ecosystem.AnimalBehaviour.PreyStates
{
  public sealed class DearStateController : AbstractPreyStateController
  {
    [SerializeField] private FoodConsumer foodConsumer;

    public override void Start()
    {
      Consumer = foodConsumer;
      base.Start();
      LookingForFood = PreyStateFactory.CreateDeerLookingForFood(Data);
      RunningTowardsFood = PreyStateFactory.CreateDeerRunningTowardsFood(Data);
      LookingForMate = PreyStateFactory.CreateDeerLookingForMate(Data);
    }
  }
}