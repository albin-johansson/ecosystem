using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public sealed class WolfStateController : AbstractStateController
  {
    [SerializeField] private PreyConsumer consumer;
    [SerializeField] private WaterConsumer waterConsumer;
    [SerializeField] private MovementController movementController;
    [SerializeField] private EcoAnimationController animationController;
    [SerializeField] private MemoryController memoryController;

    private IAnimalState _idle;
    private IAnimalState _lookingForPrey;
    private IAnimalState _lookingForWater;
    private IAnimalState _drinking;
    private IAnimalState _runningTowardsWater;
    private IAnimalState _chasingPrey;
    private WolfStateData _stateData;

    public override void Start()
    {
      _stateData = new WolfStateData
      {
              consumer = consumer,
              animationController = animationController,
              memoryController = memoryController,
              movementController = movementController,
              waterConsumer = waterConsumer
      };

      _idle = WolfStateFactory.CreateIdle(_stateData);
      _lookingForPrey = WolfStateFactory.CreateLookingForPrey(_stateData);
      _lookingForWater = WolfStateFactory.CreateLookingForWater(_stateData);
      _drinking = WolfStateFactory.CreateDrinking(_stateData);
      _runningTowardsWater = WolfStateFactory.CreateRunningTowardsWater(_stateData);
      _chasingPrey = WolfStateFactory.CreateChasingPrey(_stateData);
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

        case AnimalState.LookingForPrey:
          State = _lookingForPrey;
          break;

        case AnimalState.Idle:
          State = _idle;
          break;

        case AnimalState.Fleeing:

          break;

        case AnimalState.Drinking:
          State = _drinking;
          break;

        case AnimalState.RunningTowardsWater:
          State = _runningTowardsWater;
          break;

        case AnimalState.RunningTowardsFood:
          break;

        case AnimalState.ChasingPrey:
          State = _chasingPrey;
          break;

        default:
          break;
      }

      State.Begin(target);
    }
  }
}