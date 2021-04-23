using Ecosystem.AnimalBehaviour.Predators.Bear;
using Ecosystem.AnimalBehaviour.Predators.Wolf;

namespace Ecosystem.AnimalBehaviour.Predators
{
  public static class PredatorStateFactory
  {
    #region Common predator factory functions

    public static IAnimalState CreatePredatorIdle(StateData data)
    {
      return new PredatorIdleState(data);
    }

    public static IAnimalState CreatePredatorLookingForWater(StateData data)
    {
      return new PredatorLookingForWaterState(data);
    }

    public static IAnimalState CreatePredatorDrinking(StateData data)
    {
      return new PredatorDrinkingState(data);
    }

    public static IAnimalState CreatePredatorRunningTowardsWater(StateData data)
    {
      return new PredatorRunningTowardsWaterState(data);
    }

    public static IAnimalState CreatePredatorAttackingState(StateData data)
    {
      return new PredatorAttackingState(data);
    }

    #endregion

    #region Bear factory functions

    public static IAnimalState CreateBearRunningTowardsFood(StateData data)
    {
      return new BearRunningTowardsFoodState(data);
    }
    public static IAnimalState CreateBearChasingPrey(StateData data)
    {
      return new BearChasingPreyState(data);
    }

    public static IAnimalState CreateBearLookingForMate(StateData data)
    {
      return new BearLookingForMateState(data);
    }

    public static IAnimalState CreateBearLookingForFood(StateData data)
    {
      return new BearLookingForFoodState(data);
    }

    public static IAnimalState CreateBearEating(StateData data)
    {
      return new BearEatingState(data);
    }

    #endregion

    #region Wolf factory functions

    public static IAnimalState CreateWolfRunningTowardsFood(StateData data)
    {
      return new WolfRunningTowardsFoodState(data);
    }

    public static IAnimalState CreateWolfLookingForFood(StateData data)
    {
      return new WolfLookingForFoodState(data);
    }

    public static IAnimalState CreateWolfChasingPrey(StateData data)
    {
      return new WolfChasingPreyState(data);
    }

    public static IAnimalState CreateWolfLookingForMate(StateData data)
    {
      return new WolfLookingForMateState(data);
    }

    #endregion
  }
}