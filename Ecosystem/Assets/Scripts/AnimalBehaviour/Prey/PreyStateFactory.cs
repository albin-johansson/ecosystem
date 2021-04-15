using Ecosystem.AnimalBehaviour.Prey.Deer;
using Ecosystem.AnimalBehaviour.Prey.Rabbit;

namespace Ecosystem.AnimalBehaviour.Prey
{
  public static class PreyStateFactory
  {
    public static IAnimalState CreatePreyIdle(StateData data)
    {
      return new PreyIdleState(data);
    }

    public static IAnimalState CreateRabbitLookingForFood(StateData data)
    {
      return new RabbitLookingForFoodState(data);
    }

    public static IAnimalState CreatePreyLookingForWater(StateData data)
    {
      return new PreyLookingForWaterState(data);
    }

    public static IAnimalState CreateRabbitRunningTowardsFood(StateData data)
    {
      return new RabbitRunningTowardsFoodState(data);
    }

    public static IAnimalState CreatePreyDrinking(StateData data)
    {
      return new PreyDrinkingState(data);
    }

    public static IAnimalState CreatePreyRunningTowardsWater(StateData data)
    {
      return new PreyRunningTowardsWaterState(data);
    }

    public static IAnimalState CreatePreyFleeing(StateData data)
    {
      return new PreyFleeingState(data);
    }

    public static IAnimalState CreateRabbitLookingForMate(StateData data)
    {
      return new RabbitLookingForMateState(data);
    }

    public static IAnimalState CreateDeerLookingForMate(StateData data)
    {
      return new DeerLookingForMateState(data);
    }

    public static IAnimalState CreateDeerRunningTowardsFood(StateData data)
    {
      return new DeerRunningTowardsFoodState(data);
    }

    public static IAnimalState CreateDeerLookingForFood(StateData data)
    {
      return new DeerLookingForFoodState(data);
    }

    public static IAnimalState CreatePreyEating(StateData data)
    {
      return new PreyEatingState(data);
    }
  }
}