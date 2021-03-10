using System;

namespace Ecosystem.Genes
{
  [Serializable]
  public enum GeneType
  {
    HungerRate,
    HungerThreshold,
    ThirstRate,
    ThirstThreshold,
    Vision,
    SpeedFactor,
    SizeFactor,
    DesirabilityScore,
    GestationPeriod,
    SexualMaturityTime
  }
}