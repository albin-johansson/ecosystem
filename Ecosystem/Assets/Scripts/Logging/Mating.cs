using System;
using System.Collections.Generic;

namespace Ecosystem.Logging
{
  /// <summary>
  ///   Provides information about a mating event.
  /// </summary>
  [Serializable]
  public sealed class Mating
  {
    public List<GeneInfo> male;
    public List<GeneInfo> female;
  }
}