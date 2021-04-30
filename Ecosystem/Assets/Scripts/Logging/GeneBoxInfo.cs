using System;
using System.Collections.Generic;
using Ecosystem.Genes;

namespace Ecosystem.Logging
{
  [Serializable]
  public struct GeneBoxInfo
  {
    public List<float> value; // TODO please rename to values
    public long entryTime;
  }
}