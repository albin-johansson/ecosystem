using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public sealed class RabbitStateController : AbstractStateController
  {
    [SerializeField] private FoodConsumer consumer;
    [SerializeField] private WaterConsumer waterConsumer;
    [SerializeField] private MovementController movementController;
    [SerializeField] private EcoAnimationController animationController;
    [SerializeField] private MemoryController memoryController;

    private IAnimalState _idle;
    private IAnimalState _lookingForFood;
    private IAnimalState _lookingForWater;
    private IAnimalState _drinking;
    private IAnimalState _runningTowardsWater;
    private IAnimalState _runningTowardsFood;
    private IAnimalState _fleeing;

    private RabbitStateData _stateData;

    public override void Start()
    {
      _stateData = new RabbitStateData
      {
              Consumer = consumer,
              AnimationController = animationController,
              MemoryController = memoryController,
              MovementController = movementController,
              WaterConsumer = waterConsumer
      };

      _idle = RabbitStateFactory.CreateIdle(_stateData);
      _lookingForFood = RabbitStateFactory.CreateLookingForFood(_stateData);
      _lookingForWater = RabbitStateFactory.CreateLookingForWater(_stateData);
      _drinking = RabbitStateFactory.CreateDrinking(_stateData);
      _runningTowardsWater = RabbitStateFactory.CreateRunningTowardsWater(_stateData);
      _runningTowardsFood = RabbitStateFactory.CreateRunningTowardsFood(_stateData);
      _fleeing = RabbitStateFactory.CreateFleeing(_stateData);
      State = _idle;

      SwitchState(AnimalState.Idle);
    }

    public override void SwitchState(AnimalState state)
    {
      var target = State.End();
      switch (state)
      {
        case AnimalState.LookingForWater:
          State = _lookingForWater;
          break;

        case AnimalState.LookingForFood:
          State = _lookingForFood;
          break;

        case AnimalState.Idle:
          State = _idle;
          break;

        case AnimalState.Fleeing:
          State = _fleeing;
          break;

        case AnimalState.Drinking:
          State = _drinking;
          break;

        case AnimalState.RunningTowardsWater:
          State = _runningTowardsWater;
          break;

        case AnimalState.RunningTowardsFood:
          State = _runningTowardsFood;
          break;

        case AnimalState.LookingForPrey:
          break;

        case AnimalState.ChasingPrey:
          break;
        
        default:
          break;
      }

      State.Begin(target);
    }
  }
}