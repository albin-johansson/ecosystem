namespace Ecosystem.AnimalBehaviour.Predators
{
  public abstract class AbstractPredatorStateController : AbstractStateController
  {
    private IAnimalState _idle;
    private IAnimalState _lookingForWater;
    private IAnimalState _drinking;
    private IAnimalState _runningTowardsWater;
    private IAnimalState _attacking;

    protected IAnimalState RunningTowardsFood;
    protected IAnimalState ChasingPrey;
    protected IAnimalState LookingForMate;
    protected IAnimalState LookingForFood;
    protected IAnimalState Eating;

    protected StateData Data;

    protected override void Initialize()
    {
      base.Initialize();

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

      _idle = PredatorStateFactory.CreatePredatorIdle(Data);
      _lookingForWater = PredatorStateFactory.CreatePredatorLookingForWater(Data);
      _drinking = PredatorStateFactory.CreatePredatorDrinking(Data);
      _runningTowardsWater = PredatorStateFactory.CreatePredatorRunningTowardsWater(Data);
      _attacking = PredatorStateFactory.CreatePredatorAttackingState(Data);

      sphereCollider.radius = genome.GetVision().Value;

      State = _idle;
      stateText.SetText(State.Type());

      genderIcon.SetGenderIcon();
    }

    protected override void SwitchState(AnimalState state)
    {
      var target = State.End();
      switch (state)
      {
        case AnimalState.LookingForWater:
          State = _lookingForWater;
          break;

        case AnimalState.Idle:
          State = _idle;
          break;

        case AnimalState.Drinking:
          State = _drinking;
          break;

        case AnimalState.RunningTowardsWater:
          State = _runningTowardsWater;
          break;

        case AnimalState.ChasingPrey:
          State = ChasingPrey;
          Data.Consumer.ColliderActive = true;
          break;

        case AnimalState.LookingForFood:
          State = LookingForFood;
          Data.Consumer.ColliderActive = true;
          break;

        case AnimalState.LookingForMate:
          State = LookingForMate;
          break;

        case AnimalState.Attacking:
          State = _attacking;
          break;

        case AnimalState.Eating:
          State = Eating;
          break;

        case AnimalState.RunningTowardsFood:
          State = RunningTowardsFood;
          Data.Consumer.ColliderActive = true;
          break;
        case AnimalState.Fleeing:
        default:
          break;
      }

      State.Begin(target);
    }
  }
}