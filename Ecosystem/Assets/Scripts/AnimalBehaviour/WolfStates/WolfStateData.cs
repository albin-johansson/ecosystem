namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public class WolfStateData
  {
    public WolfStateData(PreyConsumer consumer, WaterConsumer waterConsumer, MovementController movementController, EcoAnimationController animationController, MemoryController memoryController)
    {
      this.consumer = consumer;
      this.waterConsumer = waterConsumer;
      this.movementController = movementController;
      this.animationController = animationController;
      this.memoryController = memoryController;
    }

    public PreyConsumer consumer; 
    public WaterConsumer waterConsumer;
    public MovementController movementController;
    public EcoAnimationController animationController;
    public MemoryController memoryController;
  }
}
