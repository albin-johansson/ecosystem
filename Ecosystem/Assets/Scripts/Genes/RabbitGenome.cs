using UnityEngine;

namespace Ecosystem.Genes
{
  public sealed class RabbitGenome : Genome
  {
    private static readonly Gene HungerRate = new Gene(1, 0.5f, 10);
    private static readonly Gene HungerThreshold = new Gene(5, 0, 10);
    private static readonly Gene ThirstRate = new Gene(1, 0.5f, 10);
    private static readonly Gene ThirstThreshold = new Gene(5, 0, 10);
    private static readonly Gene Vision = new Gene(25, 1, 50);
    private static readonly Gene SpeedFactor = new Gene(1.5f, 1, 2);
    private static readonly Gene SizeFactor = new Gene(0.5f, 0.1f, 1);
    private static readonly Gene DesirabilityFactor = new Gene(1, 1, 10);
    private static bool isMale;
    
    protected override void Initialize()
    {
      Initialize(0.05);
    }

    protected override void Initialize(double mutateChance)
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
      
      if(Random.value > 0.5)
      {
        Debug.Log("Created male");
        isMale = true;
      }
      else
      {
        Debug.Log("Created female");
      }
    }
  }
}