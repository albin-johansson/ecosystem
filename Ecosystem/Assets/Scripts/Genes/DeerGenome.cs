using System.Collections.Generic;

namespace Ecosystem.Genes
{
  public sealed class DeerGenome : AbstractGenome
  {
    private static readonly Gene HungerRate = new Gene(1, 0.5f, 10);
    private static readonly Gene HungerThreshold = new Gene(5, 0, 10);
    private static readonly Gene ThirstRate = new Gene(1, 0.5f, 10);
    private static readonly Gene ThirstThreshold = new Gene(5, 0, 10);
    private static readonly Gene Vision = new Gene(25, 1, 50);
    private static readonly Gene SpeedFactor = new Gene(1.5f, 1, 2);
    private static readonly Gene SizeFactor = new Gene(0.5f, 0.1f, 1);
    private static readonly Gene DesirabilityFactor = new Gene(1, 1, 10);
    private static readonly Gene GestationPeriod = new Gene(12, 10, 120);
    private static readonly Gene SexualMaturityTime = new Gene(20, 10, 150);

    private static readonly Dictionary<GeneType, Gene> DefaultGenes = new Dictionary<GeneType, Gene>
    {
            {GeneType.HungerRate, HungerRate},
            {GeneType.HungerThreshold, HungerThreshold},
            {GeneType.ThirstRate, ThirstRate},
            {GeneType.ThirstThreshold, ThirstThreshold},
            {GeneType.Vision, Vision},
            {GeneType.SpeedFactor, SpeedFactor},
            {GeneType.SizeFactor, SizeFactor},
            {GeneType.DesirabilityScore, DesirabilityFactor},
            {GeneType.GestationPeriod, GestationPeriod},
            {GeneType.SexualMaturityTime, SexualMaturityTime},
    };

    protected override void Initialize()
    {
      Data = GenomeData.Create(DefaultGenes);
    }

    public override Dictionary<GeneType, Gene> GetInitialGenes()
    {
      return DefaultGenes;
    }
  }
}