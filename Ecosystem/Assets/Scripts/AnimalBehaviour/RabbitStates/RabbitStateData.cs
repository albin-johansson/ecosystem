namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public struct RabbitStateData
  {
    public IConsumer Consumer;
    public WaterConsumer WaterConsumer;
    public MovementController MovementController;
    public EcoAnimationController AnimationController;
    public MemoryController MemoryController;
  }
}