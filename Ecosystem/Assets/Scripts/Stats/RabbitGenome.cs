using System.Collections.Generic;

public sealed class RabbitGenome : Genome
{
  public RabbitGenome() : this(0.05)
  {
  }

  public RabbitGenome(double mutateChance) : base(mutateChance)
  {
    Genes = new Dictionary<GeneType, Gene>();
    Genes.Add(GeneType.HungerRate, new Gene(1, 0.5, 10));
    Genes.Add(GeneType.HungerThreshold, new Gene(5, 0, 10));
    Genes.Add(GeneType.ThirstRate, new Gene(1, 0.5, 10));
    Genes.Add(GeneType.ThirstThreshold, new Gene(5, 0, 10));
    Genes.Add(GeneType.Vision, new Gene(25, 1, 50));
    Genes.Add(GeneType.SpeedFactor, new Gene(1.5, 1, 2));
    Genes.Add(GeneType.SizeFactor, new Gene(0.5, 0.1, 1));
    Genes.Add(GeneType.DesirabilityScore, new Gene(1, 1, 10));
  }
}