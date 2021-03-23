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
    [SerializeField] private Reproducer reproducer;

    private IAnimalState _idle;
    private IAnimalState _lookingForPrey;
    private IAnimalState _lookingForWater;
    private IAnimalState _drinking;
    private IAnimalState _runningTowardsWater;
    private IAnimalState _chasingPrey;
    private IAnimalState _lookingForMate;

    public override void Start()
    {
      var data = new WolfStateData
      {
              Consumer = consumer,
              AnimationController = animationController,
              MemoryController = memoryController,
              MovementController = movementController,
              WaterConsumer = waterConsumer,
              Reproducer = reproducer,
      };

      _idle = WolfStateFactory.CreateIdle(data);
      _lookingForPrey = WolfStateFactory.CreateLookingForPrey(data);
      _lookingForWater = WolfStateFactory.CreateLookingForWater(data);
      _drinking = WolfStateFactory.CreateDrinking(data);
      _runningTowardsWater = WolfStateFactory.CreateRunningTowardsWater(data);
      _chasingPrey = WolfStateFactory.CreateChasingPrey(data);
      _lookingForMate = WolfStateFactory.CreateLookingForMate(data);

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

        case AnimalState.LookingForFood:
          State = _lookingForPrey;
          break;

        default:
          break;
      }

      State.Begin(target);
    }
  }
}