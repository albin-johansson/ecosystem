namespace Ecosystem.AnimalBehaviour.Prey
{
  public abstract class AbstractPreyStateController : AbstractStateController
  {
    protected StateData Data;

    private IAnimalState _idle;
    private IAnimalState _lookingForWater;
    private IAnimalState _drinking;
    private IAnimalState _runningTowardsWater;
    private IAnimalState _fleeing;
    private IAnimalState _eating;

    protected IAnimalState RunningTowardsFood;
    protected IAnimalState LookingForMate;
    protected IAnimalState LookingForFood;

    private void Start()
    {
      Data = new StateData
      {
        StaminaController = staminaController,
        Consumer = Consumer,
        AnimationController = animationController,
        MemoryController = memoryController,
        MovementController = movementController,
        WaterConsumer = waterConsumer,
        Reproducer = reproducer,
        Genome = genome,
      };

      _idle = PreyStateFactory.CreatePreyIdle(Data);
      _lookingForWater = PreyStateFactory.CreatePreyLookingForWater(Data);
      _drinking = PreyStateFactory.CreatePreyDrinking(Data);
      _runningTowardsWater = PreyStateFactory.CreatePreyRunningTowardsWater(Data);
      _fleeing = PreyStateFactory.CreatePreyFleeing(Data);
      _eating = PreyStateFactory.CreatePreyEating(Data);

      sphereCollider.radius = genome.GetVision().Value;

      State = _idle;
      stateText.SetText(State.Type().ToString());
    }

    protected override void SwitchState(AnimalState state)
    {
      var target = State.End();
      switch (state)
      {
        case AnimalState.LookingForWater:
          State = _lookingForWater;
          break;

        case AnimalState.LookingForFood:
          State = LookingForFood;
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
          State = RunningTowardsFood;
          break;

        case AnimalState.LookingForMate:
          State = LookingForMate;
          break;

        case AnimalState.Eating:
          State = _eating;
          break;

        case AnimalState.ChasingPrey:
        case AnimalState.LookingForPrey:
        case AnimalState.Attacking:
        default:
          break;
      }

      State.Begin(target);
    }
  }
}
