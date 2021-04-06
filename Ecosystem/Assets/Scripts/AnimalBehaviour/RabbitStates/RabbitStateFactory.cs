namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public static class RabbitStateFactory
  {
    public static IAnimalState CreateIdle(RabbitStateData data)
    {
      return new RabbitIdleState(data);
    }

    public static IAnimalState CreateLookingForFood(RabbitStateData data)
    {
      return new RabbitLookingForFoodState(data);
    }

    public static IAnimalState CreateLookingForWater(RabbitStateData data)
    {
      return new RabbitLookingForWaterState(data);
    }

    public static IAnimalState CreateRunningTowardsFood(RabbitStateData data)
    {
      return new RabbitRunningTowardsFoodState(data);
    }

    public static IAnimalState CreateDrinking(RabbitStateData data)
    {
      return new RabbitDrinkingState(data);
    }

    public static IAnimalState CreateRunningTowardsWater(RabbitStateData data)
    {
      return new RabbitRunningTowardsWaterState(data);
    }

    public static IAnimalState CreateFleeing(RabbitStateData data)
    {
      return new RabbitFleeingState(data);
    }

    public static IAnimalState CreateLookingForMate(RabbitStateData data)
    {
      return new RabbitLookingForMateState(data);
    }

    public static IAnimalState CreateEating(RabbitStateData data)
    {
      return new RabbitEatingState(data);
    }
  }
}