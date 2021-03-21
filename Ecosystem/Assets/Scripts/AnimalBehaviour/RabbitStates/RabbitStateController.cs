using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitStateController : AbstractStateController
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
    private RabbitStateData stateData;


    public override void Start()
    {
      stateData = new RabbitStateData(consumer, waterConsumer, movementController, animationController, memoryController);
      _idle = RabbitStateFactory.CreateIdle(stateData);
      _lookingForFood = RabbitStateFactory.CreateLookingForFood(stateData);
      _lookingForWater = RabbitStateFactory.CreateLookingForWater(stateData);
      _drinking = RabbitStateFactory.CreateDrinking(stateData);
      _runningTowardsWater = RabbitStateFactory.CreateRunningTowardsWater(stateData);
      _runningTowardsFood = RabbitStateFactory.CreateRunningTowardsFood(stateData);
      _fleeing = RabbitStateFactory.CreateFleeing(stateData);
      _state = _idle;

      SwitchState(AnimalState.Idle);
    }

    public override void SwitchState(AnimalState state)
    {
      var target = _state.End();
      switch (state)
      { 
        case  AnimalState.LookingForWater:
          _state = _lookingForWater;
          break;

        case  AnimalState.LookingForFood:
          _state = _lookingForFood;
          break;

        case  AnimalState.Idle:
          _state = _idle;
          break;

        case  AnimalState.Fleeing:
          _state = _fleeing;
          break;
          
        case  AnimalState.Drinking:
          _state = _drinking;
          break;

        case  AnimalState.RunningTowardsWater:
          _state = _runningTowardsWater;
          break;

        case  AnimalState.RunningTowardsFood:
          _state = _runningTowardsFood;
          break;

        default:
          break;
      
      }
      Debug.Log(_state.Type());
      _state.Begin(target);
    }

  }
}
