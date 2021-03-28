namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public static class WolfStateFactory
  {
    public static IAnimalState CreateIdle(WolfStateData data)
    {
      return new WolfIdleState(data);
    }

    public static IAnimalState CreateLookingForPrey(WolfStateData data)
    {
      return new WolfLookingForPreyState(data);
    }

    public static IAnimalState CreateLookingForWater(WolfStateData data)
    {
      return new WolfLookingForWaterState(data);
    }

    public static IAnimalState CreateChasingPrey(WolfStateData data)
    {
      return new WolfChasingPreyState(data);
    }

    public static IAnimalState CreateDrinking(WolfStateData data)
    {
      return new WolfDrinkingState(data);
    }

    public static IAnimalState CreateRunningTowardsWater(WolfStateData data)
    {
      return new WolfRunningTowardsWaterState(data);
    }

    public static IAnimalState CreateLookingForMate(WolfStateData data)
    {
      return new WolfLookingForMateState(data);
    }

    public static IAnimalState CreateAttackingState(WolfStateData data)
    {
      return new WolfAttackingState(data);
    }
  }
}