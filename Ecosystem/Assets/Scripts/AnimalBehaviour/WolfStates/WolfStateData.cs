namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public struct WolfStateData
  {
    public PreyConsumer consumer; 
    public WaterConsumer waterConsumer;
    public MovementController movementController;
    public EcoAnimationController animationController;
    public MemoryController memoryController;
  }
}
