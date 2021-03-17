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
    internal Gene SpeedFactor;
    internal Gene SizeFactor;
    internal Gene DesirabilityScore;
    internal Gene GestationPeriod;
    internal Gene SexualMaturityTime;

    internal double MutateChance = 0.05;
    internal readonly float MetabolismFactor = 1.495f; // 1.15 (Vision) * 1.30 (Speed)
    internal readonly float ChildFoodConsumtionFactor = 1 / 3;
    internal readonly bool IsMale = Random.value > 0.5;

    internal static GenomeData Create(Dictionary<GeneType, Gene> initial) => new GenomeData
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