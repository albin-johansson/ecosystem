using System;
using Ecosystem.Genes;

namespace Ecosystem.Logging
{
  /// <summary>
  ///   Provides information about a gene.
  /// </summary>
  /// <remarks>
  ///   Note, the Unity JSON serializer seems to struggle a bit with properties, so
  ///   we cannot just provide a <c>Gene</c> field here.
  /// </remarks>
  [Serializable]
  public struct GeneInfo
  {
    public GeneType type;
    public float min;
    public float max;
    public float value;
  }
}