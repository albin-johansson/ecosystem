using Ecosystem.Genes;
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
    [SerializeField] private Reproducer reproducer;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private AbstractGenome genome;

    private IAnimalState _idle;
    private IAnimalState _lookingForFood;
    private IAnimalState _lookingForWater;
    private IAnimalState _drinking;
    private IAnimalState _runningTowardsWater;
    private IAnimalState _runningTowardsFood;
    private IAnimalState _fleeing;
    private IAnimalState _lookingForMate;

    public override void Start()
    {
      var data = new RabbitStateData
      {
              Consumer = consumer,
              AnimationController = animationController,
              MemoryController = memoryController,
              MovementController = movementController,
              WaterConsumer = waterConsumer,
              Reproducer = reproducer
      };

      _idle = RabbitStateFactory.CreateIdle(data);
      _lookingForFood = RabbitStateFactory.CreateLookingForFood(data);
      _lookingForWater = RabbitStateFactory.CreateLookingForWater(data);
      _drinking = RabbitStateFactory.CreateDrinking(data);
      _runningTowardsWater = RabbitStateFactory.CreateRunningTowardsWater(data);
      _runningTowardsFood = RabbitStateFactory.CreateRunningTowardsFood(data);
      _fleeing = RabbitStateFactory.CreateFleeing(data);
      _lookingForMate = RabbitStateFactory.CreateLookingForMate(data);
      
      sphereCollider.radius = (sphereCollider.radius / sphereCollider.transform.lossyScale.magnitude) * genome.GetVision().Value;

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
        
        case AnimalState.LookingForMate:
          State = _lookingForMate;
          break;

        default:
          break;
      }

      State.Begin(target);
    }
  }
}