using System.Collections.Generic;

namespace Ecosystem.Genes
{
  public sealed class BearGenome : AbstractGenome
  {
    private static readonly Gene HungerRate = new Gene(0.2f, 0.1f, 1f);
    private static readonly Gene HungerThreshold = new Gene(25, 0, 50);
    private static readonly Gene ThirstRate = new Gene(0.3f, 0.1f, 1f);
    private static readonly Gene ThirstThreshold = new Gene(25, 0, 50);
    private static readonly Gene Vision = new Gene(8f, 4f, 12f);
    private static readonly Gene Speed = new Gene(2f, 1f, 5f);
    private static readonly Gene SizeFactor = new Gene(1, 0.5f, 1.5f);
    private static readonly Gene DesirabilityFactor = new Gene(0.5f, 0, 1);
    private static readonly Gene GestationPeriod = new Gene(60, 20, 120);
    private static readonly Gene SexualMaturityTime = new Gene(20, 10, 120);

    public static readonly Dictionary<GeneType, Gene> DefaultGenes = new Dictionary<GeneType, Gene>
    {
      {GeneType.HungerRate, HungerRate},
      {GeneType.HungerThreshold, HungerThreshold},
      {GeneType.ThirstRate, ThirstRate},
      {GeneType.ThirstThreshold, ThirstThreshold},
      {GeneType.Vision, Vision},
      {GeneType.Speed, Speed},
      {GeneType.SizeFactor, SizeFactor},
      {GeneType.DesirabilityScore, DesirabilityFactor},
      {GeneType.GestationPeriod, GestationPeriod},
      {GeneType.SexualMaturityTime, SexualMaturityTime},
    };

    protected override void Initialize()
    {
      Data = GenomeData.Create(DefaultGenes);
      ConvertGenesToAttributes();
    }
  }
}