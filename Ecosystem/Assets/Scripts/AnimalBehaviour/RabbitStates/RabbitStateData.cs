namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public class RabbitStateData
  {
    public RabbitStateData(FoodConsumer consumer, WaterConsumer waterConsumer, MovementController movementController, EcoAnimationController animationController, MemoryController memoryController)
    {
      this.consumer = consumer;
      this.waterConsumer = waterConsumer;
      this.movementController = movementController;
      this.animationController = animationController;
      this.memoryController = memoryController;
    }
    public IConsumer consumer; 
    public WaterConsumer waterConsumer;
    public MovementController movementController;
    public EcoAnimationController animationController;
    public MemoryController memoryController;
  }
}
