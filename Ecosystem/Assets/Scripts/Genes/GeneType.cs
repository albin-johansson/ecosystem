﻿using System;

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
    Speed,
    SizeFactor,
    DesirabilityScore,
    GestationPeriod,
    SexualMaturityTime
  }
}