using UnityEngine;

namespace Ecosystem.AnimalBehaviour.Predators
{
  public abstract class AbstractPredatorStateController : AbstractStateController
  {
    [SerializeField] private PreyConsumer consumer;

    private IAnimalState _idle;
    private IAnimalState _lookingForWater;
    private IAnimalState _drinking;
    private IAnimalState _runningTowardsWater;
    private IAnimalState _attacking;
    private IAnimalState _runningTowardsFood;

    protected IAnimalState ChasingPrey;
    protected IAnimalState LookingForMate;
    protected IAnimalState LookingForFood;

    protected StateData Data;

    protected override void Initialize()
    {
      base.Initialize();

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
      _runningTowardsFood = PredatorStateFactory.CreateWolfRunningTowardsFood(Data);

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
          consumer.ColliderActive = true;
          break;

        case AnimalState.LookingForFood:
          State = LookingForFood;
          consumer.ColliderActive = true;
          break;

        case AnimalState.LookingForMate:
          State = LookingForMate;
          break;

        case AnimalState.Attacking:
          State = _attacking;
          break;
        
        case AnimalState.GoingToFood:
          State = _runningTowardsFood;
          consumer.ColliderActive = true;
          break;

        case AnimalState.Fleeing:
        case AnimalState.Eating:
        case AnimalState.RunningTowardsFood:
        default:
          break;
      }

      State.Begin(target);
    }
  }
}