using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Prey.Deer
{
  public sealed class DeerStateController : AbstractPreyStateController
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