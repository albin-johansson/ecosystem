using System;
using Ecosystem.Genes;

namespace Ecosystem.Logging
{
  [Serializable]
  public struct GeneAverageInfo
  {
    public float value;
    public long entryTime;
  }
}