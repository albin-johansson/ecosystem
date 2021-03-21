namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public static class RabbitStateFactory
  {
    public static IAnimalState CreateIdle(RabbitStateData data)
    {
      var State = new RabbitIdleState(data);
      return State;
    }

    public static IAnimalState CreateLookingForFood(RabbitStateData data)
    {
      RabbitLookingForFoodState State = new RabbitLookingForFoodState(data);
      return State;
    }

    public static IAnimalState CreateLookingForWater(RabbitStateData data)
    {
      RabbitLookingForWaterState State = new RabbitLookingForWaterState(data);
      return State;
    }

    public static IAnimalState CreateRunningTowardsFood(RabbitStateData data)
    {
      RabbitRunningTowardsFoodState State = new RabbitRunningTowardsFoodState(data);
      return State;
    }

    public static IAnimalState CreateDrinking(RabbitStateData data)
    {
      RabbitDrinkingState State = new RabbitDrinkingState(data);
      return State;
    }

    public static IAnimalState CreateRunningTowardsWater(RabbitStateData data)
    {
      RabbitRunningTowardsWaterState State = new RabbitRunningTowardsWaterState(data);
      return State;
    }

    public static IAnimalState CreateFleeing(RabbitStateData data)
    {
      RabbitFleeingState State = new RabbitFleeingState(data);
      return State;
    }
  }
}
