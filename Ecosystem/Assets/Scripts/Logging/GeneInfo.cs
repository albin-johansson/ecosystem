using System;
using Ecosystem.Genes;

namespace Ecosystem.Logging
{
  /// <summary>
  ///   Provides information about a gene.
  /// </summary>
  [Serializable]
  public struct GeneInfo
  {
    public GeneType gene;
    public float value;
  }
}