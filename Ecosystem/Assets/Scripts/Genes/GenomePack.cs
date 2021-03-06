using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Ecosystem.Genes
{
  public sealed class GenomePack
  {
    internal Gene HungerRate;
    internal Gene HungerThreshold;
    internal Gene ThirstRate;
    internal Gene ThirstThreshold;
    internal Gene Vision;
    internal Gene SpeedFactor;
    internal Gene SizeFactor;
    internal Gene DesirabilityScore;
    internal Gene GestationPeriod;
    internal Gene SexualMaturityTime;

    internal double MutateChance = 0.05;
    internal readonly bool IsMale = Random.value > 0.5;

    internal static GenomePack Create(Dictionary<GeneType, Gene> initial) => new GenomePack
    {
            HungerRate = initial[GeneType.HungerRate],
            HungerThreshold = initial[GeneType.HungerThreshold],
            ThirstRate = initial[GeneType.ThirstRate],
            ThirstThreshold = initial[GeneType.ThirstThreshold],
            Vision = initial[GeneType.Vision],
            SpeedFactor = initial[GeneType.SpeedFactor],
            SizeFactor = initial[GeneType.SizeFactor],
            DesirabilityScore = initial[GeneType.DesirabilityScore],
            GestationPeriod = initial[GeneType.GestationPeriod],
            SexualMaturityTime = initial[GeneType.SexualMaturityTime]
    };
  }
}