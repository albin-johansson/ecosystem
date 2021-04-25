using System;
using System.Collections.Generic;
using Ecosystem.Genes;

namespace Ecosystem.Logging
{
  [Serializable]
  public struct GeneBoxInfo
  {
    public List<float> value;
    public long entryTime;

    public GeneBoxInfo(List<float> vals, GeneType t, string tag, long time)
    {
      value = vals;
      entryTime = time;
    }
  }
}