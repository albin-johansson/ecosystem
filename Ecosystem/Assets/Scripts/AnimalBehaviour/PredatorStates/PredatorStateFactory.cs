using Ecosystem.AnimalBehaviour.WolfStates;

namespace Ecosystem.AnimalBehaviour.PredatorStates
{
  public static class PredatorStateFactory
  {
    public static IAnimalState CreatePredatorIdle(StateData data)
    {
      return new PredatorIdleState(data);
    }

    public static IAnimalState CreateWolfLookingForPrey(StateData data)
    {
      return new WolfLookingForPreyState(data);
    }

    public static IAnimalState CreatePredatorLookingForWater(StateData data)
    {
      return new PredatorLookingForWaterState(data);
    }

    public static IAnimalState CreateWolfChasingPrey(StateData data)
    {
      return new WolfChasingPreyState(data);
    }

    public static IAnimalState CreatePredatorDrinking(StateData data)
    {
      return new PredatorDrinkingState(data);
    }

    public static IAnimalState CreatePredatorRunningTowardsWater(StateData data)
    {
      return new PredatorRunningTowardsWaterState(data);
    }

    public static IAnimalState CreateWolfLookingForMate(StateData data)
    {
      return new WolfLookingForMateState(data);
    }

    public static IAnimalState CreatePredatorAttackingState(StateData data)
    {
      return new PredatorAttackingState(data);
    }
    
    public static IAnimalState CreateBearLookingForFood(StateData data)
    {
      return new BearLookingForFoodState(data);
    }
    
    public static IAnimalState CreateBearChasingFood(StateData data)
    {
      return new BearChasingFoodState(data);
    }
    
    public static IAnimalState CreateBearLookingForMate(StateData data)
    {
      return new BearLookingForMateState(data);
    }
    
    public static IAnimalState CreateGoingToFoodState(StateData data)
    {
      return new WolfRunningTowardsFoodState(data);
    }
  }
}