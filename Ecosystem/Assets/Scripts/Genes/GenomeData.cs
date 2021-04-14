using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Genes
{
  public sealed class GenomeData
  {
    internal Gene HungerRate;
    internal Gene HungerThreshold;
    internal Gene ThirstRate;
    internal Gene ThirstThreshold;
    internal Gene Vision;
    internal Gene Speed;
    internal Gene SizeFactor;
    internal Gene DesirabilityScore;
    internal Gene GestationPeriod;
    internal Gene SexualMaturityTime;

    internal double MutateChance = 0.05;
    internal readonly bool IsMale = Random.value > 0.5;

    internal static GenomeData Create(Dictionary<GeneType, Gene> initial) => new GenomeData
    {
      HungerRate = initial[GeneType.HungerRate],
      HungerThreshold = initial[GeneType.HungerThreshold],
      ThirstRate = initial[GeneType.ThirstRate],
      ThirstThreshold = initial[GeneType.ThirstThreshold],
      Vision = initial[GeneType.Vision],
      Speed = initial[GeneType.Speed],
      SizeFactor = initial[GeneType.SizeFactor],
      DesirabilityScore = initial[GeneType.DesirabilityScore],
      GestationPeriod = initial[GeneType.GestationPeriod],
      SexualMaturityTime = initial[GeneType.SexualMaturityTime]
    };
  }
}