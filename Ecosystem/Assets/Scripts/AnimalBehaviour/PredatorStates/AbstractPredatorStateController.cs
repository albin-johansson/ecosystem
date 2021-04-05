namespace Ecosystem.AnimalBehaviour.PredatorStates
{
  public abstract class AbstractPredatorStateController : AbstractStateController
  {
    protected StateData Data;

    private IAnimalState _idle;
    private IAnimalState _lookingForWater;
    private IAnimalState _drinking;
    private IAnimalState _runningTowardsWater;
    private IAnimalState _attacking;

    protected IAnimalState ChasingPrey;
    protected IAnimalState LookingForMate;
    protected IAnimalState LookingForFood;


    public override void Start()
    {
      Data = new StateData
      {
        StaminaController = staminaController,
        Consumer = consumer,
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
          State = LookingForFood;
          Consumer.CollideActive = true;
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
          State = ChasingPrey;
          Consumer.CollideActive = true;
          break;

        case AnimalState.LookingForFood:
          State = LookingForFood;
          break;

        case AnimalState.LookingForMate:
          State = LookingForMate;
          break;

        case AnimalState.Attacking:
          State = _attacking;
          break;

        default:
          break;
      }

      State.Begin(target);
    }
  }
}
