using System;
using Ecosystem.Genes;

namespace Ecosystem.Logging
{
  [Serializable]
  public struct GeneAverageInfo
  {
    public float value;
    public long entryTime;

    public GeneAverageInfo(float val, GeneType t, string tag, long time)
    {
      value = val;
      entryTime = time;
    }
  }
}