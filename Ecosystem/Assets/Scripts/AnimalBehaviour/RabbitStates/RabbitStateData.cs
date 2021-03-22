namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public struct RabbitStateData
  {
    public IConsumer consumer; 
    public WaterConsumer waterConsumer;
    public MovementController movementController;
    public EcoAnimationController animationController;
    public MemoryController memoryController;
  }
}
