namespace Ecosystem.AnimalBehaviour
{
  public struct StateData
  {
    public IConsumer Consumer;
    public WaterConsumer WaterConsumer;
    public MovementController MovementController;
    public EcoAnimationController AnimationController;
    public MemoryController MemoryController;
    public Reproducer Reproducer;
  }
}