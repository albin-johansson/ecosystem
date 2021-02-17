using System.Collections.Generic;

public sealed class WolfGenome : Genome
{
  public override void Initialize()
  {
    Initialize(0.05);
  }

  public override void Initialize(double mutateChance)
  {
    Genes[GeneType.HungerRate] = new Gene(3, 0.5, 10);
    Genes[GeneType.HungerThreshold] = new Gene(5, 0, 10);
    Genes[GeneType.ThirstRate] = new Gene(2, 0.5, 10);
    Genes[GeneType.ThirstThreshold] = new Gene(5, 0, 10);
    Genes[GeneType.Vision] = new Gene(20, 1, 50);
    Genes[GeneType.SpeedFactor] = new Gene(15, 1, 25);
    Genes[GeneType.SizeFactor] = new Gene(0.5, 0.1, 1);
    Genes[GeneType.DesirabilityScore] = new Gene(1, 1, 10);
  }
}