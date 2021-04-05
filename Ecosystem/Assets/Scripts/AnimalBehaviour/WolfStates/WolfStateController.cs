using UnityEngine;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public sealed class WolfStateController : AbstractStateController
  {
    [SerializeField] private PreyConsumer consumer;


    private IAnimalState _idle;
    private IAnimalState _lookingForPrey;
    private IAnimalState _lookingForWater;
    private IAnimalState _drinking;
    private IAnimalState _runningTowardsWater;
    private IAnimalState _chasingPrey;
    private IAnimalState _lookingForMate;
    private IAnimalState _attacking;
    private IAnimalState _goingToFood;

    public override void Start()
    {
      var data = new WolfStateData
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

      _idle = WolfStateFactory.CreateIdle(data);
      _lookingForPrey = WolfStateFactory.CreateLookingForPrey(data);
      _lookingForWater = WolfStateFactory.CreateLookingForWater(data);
      _drinking = WolfStateFactory.CreateDrinking(data);
      _runningTowardsWater = WolfStateFactory.CreateRunningTowardsWater(data);
      _chasingPrey = WolfStateFactory.CreateChasingPrey(data);
      _lookingForMate = WolfStateFactory.CreateLookingForMate(data);
      _attacking = WolfStateFactory.CreateAttackingState(data);
      _goingToFood = WolfStateFactory.CreateGoingToFoodState(data);


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
          State = _lookingForPrey;
          consumer.CollideActive = true;
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
          consumer.CollideActive = true;
          break;

        case AnimalState.LookingForFood:
          State = _lookingForPrey;
          consumer.CollideActive = true;
          break;

        case AnimalState.LookingForMate:
          State = _lookingForMate;
          break;

        case AnimalState.Attacking:
          State = _attacking;
          break;
        
        case AnimalState.GoingToMeat:
          State = _goingToFood;
          consumer.CollideActive = true;
          break;

        default:
          break;
      }

      State.Begin(target);
    }
  }
}