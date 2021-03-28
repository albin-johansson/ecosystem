using Ecosystem.Genes;

namespace Ecosystem.AnimalBehaviour.WolfStates
{
  public struct WolfStateData
  {
    public PreyConsumer Consumer;
    public WaterConsumer WaterConsumer;
    public MovementController MovementController;
    public EcoAnimationController AnimationController;
    public MemoryController MemoryController;
    public Reproducer Reproducer;
    public AbstractGenome Genome;
  }
}