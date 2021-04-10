using System;
using System.Collections.Generic;

namespace Ecosystem.Logging
{
  [Serializable]
  public struct GenomeInfo 
  {
    /// <summary>
    ///   The time of the creation of the gene(s), relative to the start of the simulation, in milliseconds.
    /// </summary>
    public long time;

    /// <summary>
    /// When the animal with the genome died, in milliseconds. Default to TODO: something large. or set to duration. 
    /// </summary>
    public long endTime;
    /// <summary>
    ///   The tag attached to the game object associated with the event. 
    /// </summary>
    public string tag;

    /// <summary>
    ///   hungerRateGene only for testing. 
    /// </summary>
    //public GeneInfo hungerRate;

    /// <summary>
    /// All tracked genes.
    /// </summary>
    public List<GeneInfo> genes;
  }
}