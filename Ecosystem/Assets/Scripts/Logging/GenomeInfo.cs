using System;
using System.Collections.Generic;

namespace Ecosystem.Logging
{
  public sealed partial class LogData
  {
    [Serializable]
    public sealed class GenomeInfo
    {
      public List<GeneInfo> genes;
    }
  }
}