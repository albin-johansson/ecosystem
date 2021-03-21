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
    private WolfStateData stateData;


    public override void Start()
    {
      stateData = new WolfStateData(consumer, waterConsumer, movementController, animationController, memoryController);
      _idle = WolfStateFactory.CreateIdle(stateData);
      _lookingForPrey = WolfStateFactory.CreateLookingForPrey(stateData);
      _lookingForWater = WolfStateFactory.CreateLookingForWater(stateData);
      _drinking = WolfStateFactory.CreateDrinking(stateData);
      _runningTowardsWater = WolfStateFactory.CreateRunningTowardsWater(stateData);
      _chasingPrey = WolfStateFactory.CreateChasingPrey(stateData);
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

        case  AnimalState.LookingForPrey:
          _state = _lookingForPrey;
          break;

        case  AnimalState.Idle:
          _state = _idle;
          break;

        case  AnimalState.Fleeing:

          break;
          
        case  AnimalState.Drinking:
          _state = _drinking;
          break;

        case  AnimalState.RunningTowardsWater:
          _state = _runningTowardsWater;
          break;

        case  AnimalState.RunningTowardsFood:
          break;

        case  AnimalState.ChasingPrey:
          _state = _chasingPrey;
          break;

        default:
          break;
      
      }
      _state.Begin(target);
    }
  }
}
