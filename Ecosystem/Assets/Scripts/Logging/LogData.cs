using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace Ecosystem.Logging
{
  /// <summary>
  ///   Provides information about a death event.
  /// </summary>
  [Serializable]
  public sealed class Death
  {
    /// <summary>
    ///   The cause of death.
    /// </summary>
    public CauseOfDeath cause;
  }

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
    ///   Provides extra information about death events. This should only be used if
    ///   the type of the event is <c>"death"</c>. 
    /// </summary>
    /// <remarks>
    ///   The Unity JSON serializer will unfortunately always include a this information
    ///   in the JSON file, even if the field is <c>null</c>. 
    /// </remarks>
    public Death deathInfo;
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

    /// <summary>
    ///   The initial amount of rabbits.
    /// </summary>
    public int initialAliveRabbitsCount;

    /// <summary>
    ///   The initial amount of deer.
    /// </summary>
    public int initialAliveDeerCount;

    /// <summary>
    ///   The initial amount of wolves.
    /// </summary>
    public int initialAliveWolvesCount;

    /// <summary>
    ///   The initial amount of bears.
    /// </summary>
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

    /// <summary>
    ///   The amount of animals that have been born.
    /// </summary>
    public int birthCount;

    /// <summary>
    ///   The amount of prey that were consumed.
    /// </summary>
    public int preyConsumedCount;

    /// <summary>
    ///   The history of simulation events, stored in chronological order. 
    /// </summary>
    public List<SimulationEvent> events = new List<SimulationEvent>();
  }

  /// <summary>
  ///   A factory class for creating simulation events of different types.
  /// </summary>
  public static class EventFactory
  {
    /// <summary>
    ///   Creates a simulation event that represents the birth of an animal.
    /// </summary>
    /// <param name="animal">the game object associated with the animal that was born.</param>
    /// <returns>an event that describes the birth.</returns>
    public static SimulationEvent CreateBirth(GameObject animal) => new SimulationEvent
    {
            time = AnalyticsSessionInfo.sessionElapsedTime,
            type = "birth",
            tag = animal.tag,
            position = animal.transform.position
    };

    /// <summary>
    ///   Creates a simulation event that represents the death of an animal.
    /// </summary>
    /// <param name="deadObject">the game object associated with the animal that died.</param>
    /// <param name="cause">the cause of death for the animal.</param>
    /// <returns>an event that describes the death.</returns>
    public static SimulationEvent CreateDeath(GameObject deadObject, CauseOfDeath cause) => new SimulationEvent
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

    /// <summary>
    ///   Creates a simulation event that represents a food item being consumed.
    /// </summary>
    /// <param name="food">the game object associated with the food item that was consumed.</param>
    /// <returns>an event that describes the food consumption.</returns>
    public static SimulationEvent CreateConsumption(GameObject food) => new SimulationEvent
    {
            time = AnalyticsSessionInfo.sessionElapsedTime,
            type = "consumption",
            tag = food.tag,
            position = food.transform.position
    };
  }
}