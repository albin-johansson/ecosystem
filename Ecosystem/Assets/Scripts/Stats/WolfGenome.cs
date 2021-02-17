using System.Collections.Generic;

public sealed class WolfGenome : Genome
{
  private static readonly Gene HungerRate = new Gene(3, 0.5, 10);
  private static readonly Gene HungerThreshold = new Gene(5, 0, 10);
  private static readonly Gene ThirstRate = new Gene(2, 0.5, 10);
  private static readonly Gene ThirstThreshold = new Gene(5, 0, 10);
  private static readonly Gene Vision =  new Gene(20, 1, 50);
  private static readonly Gene SpeedFactor = new Gene(15, 1, 25);
  private static readonly Gene SizeFactor = new Gene(0.5, 0.1, 1);
  private static readonly Gene DesirabilityFactor = new Gene(1, 1, 10);
  public override void Initialize()
  {
    Initialize(0.05);
  }

  public override void Initialize(double mutateChance)
  {
    MutateChance = mutateChance;
    Genes[GeneType.HungerRate] = HungerRate;
    Genes[GeneType.HungerThreshold] = HungerThreshold;
    Genes[GeneType.ThirstRate] = ThirstRate;
    Genes[GeneType.ThirstThreshold] = ThirstThreshold;
    Genes[GeneType.Vision] = Vision;
    Genes[GeneType.SpeedFactor] = SpeedFactor;
    Genes[GeneType.SizeFactor] = SizeFactor;
    Genes[GeneType.DesirabilityScore] = DesirabilityFactor;
  }
}