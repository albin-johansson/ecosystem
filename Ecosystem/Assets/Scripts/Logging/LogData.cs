using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Analytics;

namespace Ecosystem.Logging
{
  /// <summary>
  ///   Provides information about a death event.
  /// </summary>
  [Serializable]
  public class Death
  {
    /// <summary>
    ///   The cause of death.
    /// </summary>
    public CauseOfDeath cause;
  }

  [Serializable]
  public struct SimulationEvent
  {
    public long time;
    public string tag;
    public string type;
    public Vector3 position;

    public Death deathInfo; // Only set for death events 
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
    ///   The initial amount of alive predators.
    /// </summary>
    public int initialAlivePredatorCount;

    /// <summary>
    ///   The initial amount of alive prey.
    /// </summary>
    public int initialAlivePreyCount;

    public int initialAliveRabbitsCount;
    public int initialAliveDeerCount;
    public int initialAliveWolvesCount;
    public int initialAliveBearsCount;

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

    public int birthCount;

    /// <summary>
    ///   The amount of prey that were consumed.
    /// </summary>
    public int preyConsumedCount;

    public List<SimulationEvent> events = new List<SimulationEvent>();
  }

  public static class EventFactory
  {
    public static SimulationEvent CreateBirth(GameObject animal)
    {
      return new SimulationEvent
      {
              time = AnalyticsSessionInfo.sessionElapsedTime,
              type = "birth",
              tag = animal.tag,
              position = animal.transform.position
      };
    }

    public static SimulationEvent CreateDeath(GameObject deadObject, CauseOfDeath cause)
    {
      return new SimulationEvent
      {
              time = AnalyticsSessionInfo.sessionElapsedTime,
              type = "death",
              tag = deadObject.tag,
              position = deadObject.transform.position,
              deathInfo = new Death
              {
                      cause = cause
              }
      };
    }

    public static SimulationEvent CreateConsumption(GameObject food)
    {
      return new SimulationEvent
      {
              time = AnalyticsSessionInfo.sessionElapsedTime,
              type = "consumption",
              tag = food.tag,
              position = food.transform.position
      };
    }
  }
}