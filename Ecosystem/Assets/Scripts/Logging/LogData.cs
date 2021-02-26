using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Logging
{
  /// <summary>
  ///   Provides information about a food consumption event.
  /// </summary>
  [Serializable]
  public struct FoodConsumption
  {
    /// <summary>
    ///   The time of death, in milliseconds since the start of the simulation.
    /// </summary>
    public long time;

    /// <summary>
    ///   The position of the animal that died.
    /// </summary>
    public Vector3 position;
  }

  /// <summary>
  ///   Provides information about a death event.
  /// </summary>
  [Serializable]
  public struct Death
  {
    /// <summary>
    ///   The time of death, in milliseconds since the start of the simulation.
    /// </summary>
    public long time;

    /// <summary>
    ///   The cause of death.
    /// </summary>
    public CauseOfDeath cause;

    /// <summary>
    ///   The position of the animal that died.
    /// </summary>
    public Vector3 position;
  }

  /// <summary>
  ///   The in-memory storage of simulation data, which can be serialized
  ///   when the simulation has finished.
  /// </summary>
  [Serializable]
  public sealed class LogData
  {
    /// <summary>
    ///   Duration of simulation, in milliseconds.
    /// </summary>
    public long duration;

    /// <summary>
    ///   The initial amount of food items.
    /// </summary>
    public int initialFoodCount;

    /// <summary>
    ///   The initial amount of alive animals.
    /// </summary>
    public int initialAliveCount;

    /// <summary>
    ///   The final amount of available food items.
    /// </summary>
    public int foodCount;

    /// <summary>
    ///   The final amount of alive animals.
    /// </summary>
    public int aliveCount;

    /// <summary>
    ///   The amount of animals that have died.
    /// </summary>
    public int deadCount;

    /// <summary>
    ///   The amount of prey that were consumed.
    /// </summary>
    public int preyConsumedCount;

    /// <summary>
    ///   The history of all food consumptions.
    /// </summary>
    public List<FoodConsumption> foodConsumptions = new List<FoodConsumption>();

    /// <summary>
    ///   The history of all deaths.
    /// </summary>
    public List<Death> deaths = new List<Death>();
  }
}