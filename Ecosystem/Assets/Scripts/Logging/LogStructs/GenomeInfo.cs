using System;
using System.Collections.Generic;
using UnityEngine;

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
    /// When the animal with the genome died, in milliseconds. 
    /// </summary>
    public long endTime;

    /// <summary>
    ///   The tag attached to the game object associated with the event. 
    /// </summary>
    public string tag;

    /// <summary>
    /// All tracked genes.
    /// </summary>
    public List<GeneInfo> genes;

    /// <summary>
    /// ID from genome. 
    /// </summary>
    public string key;
  }
}