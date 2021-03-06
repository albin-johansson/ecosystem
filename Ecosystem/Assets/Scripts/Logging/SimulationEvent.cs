using System;
using UnityEngine;

namespace Ecosystem.Logging
{
  /// <summary>
  ///   Represents an "event" in the simulation, such as animal births,
  ///   deaths and food consumption. Certain aspects are common for all events,
  ///   but there are aspects that are only relevant for specific event types.
  /// </summary>
  [Serializable]
  public struct SimulationEvent
  {
    /// <summary>
    ///   The time of the event, relative to the start of the simulation, in milliseconds.
    /// </summary>
    public long time;

    /// <summary>
    ///   The tag attached to the game object associated with the event. 
    /// </summary>
    public string tag;

    /// <summary>
    ///   The type of the simulation event.
    /// </summary>
    public string type;

    /// <summary>
    ///   The position of the event.
    /// </summary>
    public Vector3 position;

    /// <summary>
    ///   The index of the additional mating information object. 
    /// </summary>
    /// <remarks>
    ///   This is only used by mating events, it is set to <c>-1</c> otherwise.
    /// </remarks>
    public int matingIndex;
    
    /// <summary>
    ///   The index of the additional death information object. 
    /// </summary>
    /// <remarks>
    ///   This is only used by death events, it is set to <c>-1</c> otherwise.
    /// </remarks>
    public int deathIndex;
  }
}