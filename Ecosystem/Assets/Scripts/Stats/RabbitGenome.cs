using System.Collections.Generic;

public sealed class RabbitGenome : Genome
{
  public RabbitGenome()
  {
    genes = new Dictionary<GeneType, Gene>();
    genes.Add(GeneType.HungerRate, new Gene(1, 0.5, 10));
    genes.Add(GeneType.HungerThreshold, new Gene(5, 0, 10));
    genes.Add(GeneType.ThirstRate, new Gene(1, 0.5, 10));
    genes.Add(GeneType.ThirstThreshold, new Gene(5, 0, 10));
    genes.Add(GeneType.Vision, new Gene(25, 1, 50));
    genes.Add(GeneType.SpeedFactor, new Gene(1.5, 1, 2));
    genes.Add(GeneType.SizeFactor, new Gene(0.5, 0.1, 1));
    genes.Add(GeneType.DesirabilityScore, new Gene(1, 1, 10));
    mutateChance = 0.05;
  }
}