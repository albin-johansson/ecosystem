namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public static class WolfStateFactory
  {
    public static IAnimalState CreateIdle(WolfStateData data)
    {
      var State = new WolfIdleState(data);
      return State;
    }

    public static IAnimalState CreateLookingForPrey(WolfStateData data)
    {
      WolfLookingForPreyState State = new WolfLookingForPreyState(data);
      return State;
    }

    public static IAnimalState CreateLookingForWater(WolfStateData data)
    {
      WolfLookingForWaterState State = new WolfLookingForWaterState(data);

      return State;
    }

    public static IAnimalState CreateChasingPrey(WolfStateData data)
    {
      WolfChasingPreyState State = new WolfChasingPreyState(data);
      return State;
    }

    public static IAnimalState CreateDrinking(WolfStateData data)
    {
      WolfDrinkingState State = new WolfDrinkingState(data);
      return State;
    }

    public static IAnimalState CreateRunningTowardsWater(WolfStateData data)
    {
      WolfRunningTowardsWaterState State = new WolfRunningTowardsWaterState(data);
      return State;
    }
  }
}
