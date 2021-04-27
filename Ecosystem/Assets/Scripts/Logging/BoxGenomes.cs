using System;
using System.Collections.Generic;

namespace Ecosystem.Logging
{
  [Serializable]
  public struct BoxGenomes
  {
    public string animal;
    public List<GeneBoxInfo> HungerRate;
    public List<GeneBoxInfo> HungerThreshold;
    public List<GeneBoxInfo> ThirstRate;
    public List<GeneBoxInfo> ThirstThreshold;
    public List<GeneBoxInfo> Vision;
    public List<GeneBoxInfo> Speed;
    public List<GeneBoxInfo> SizeFactor;
    public List<GeneBoxInfo> DesirabilityScore;
    public List<GeneBoxInfo> GestationPeriod;
    public List<GeneBoxInfo> SexualMaturityTime;
  }
}