using System;
using Ecosystem.Genes;

namespace Ecosystem.Logging
{
  [Serializable]
  public struct GeneAverageInfo
  {
    public float value;
    //public GeneType type;
    public string animal;
    public long entryTime;

    public GeneAverageInfo(float val, GeneType t, string tag, long time)
    {
      value = val;
      //type = t;
      animal = tag;
      entryTime = time;
    }
  }
}