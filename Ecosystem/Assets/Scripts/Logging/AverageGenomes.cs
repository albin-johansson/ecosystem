using System;
using System.Collections.Generic;

namespace Ecosystem.Logging
{
  [Serializable]
  public struct AverageGenomes
  {
    public string animal;
    public List<GeneAverageInfo> HungerRate;
    public List<GeneAverageInfo> HungerThreshold;
    public List<GeneAverageInfo> ThirstRate;
    public List<GeneAverageInfo> ThirstThreshold;
    public List<GeneAverageInfo> Vision;
    public List<GeneAverageInfo> Speed;
    public List<GeneAverageInfo> SizeFactor;
    public List<GeneAverageInfo> DesirabilityScore;
    public List<GeneAverageInfo> GestationPeriod;
    public List<GeneAverageInfo> SexualMaturityTime;
  }
}