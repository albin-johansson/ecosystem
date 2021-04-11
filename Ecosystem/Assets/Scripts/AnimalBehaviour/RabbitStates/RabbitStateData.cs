using Ecosystem.Genes;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  public struct RabbitStateData
  {
    public StaminaController StaminaController;
    public IConsumer Consumer;
    public WaterConsumer WaterConsumer;
    public MovementController MovementController;
    public EcoAnimationController AnimationController;
    public MemoryController MemoryController;
    public Reproducer Reproducer;
    public AbstractGenome Genome;
  }
}