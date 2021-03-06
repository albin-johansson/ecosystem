using System;
using System.Collections.Generic;
using Ecosystem.Genes;
using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.Logging
{
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
    [SerializeField] private long duration;

    /// <summary>
    ///   The initial amount of food items.
    /// </summary>
    [SerializeField] private int initialFoodCount;

    /// <summary>
    ///   The initial amount of alive animals.
    /// </summary>
    [SerializeField] private int initialAliveCount;

    /// <summary>
    ///   The initial amount of alive predators.
    /// </summary>
    [SerializeField] private int initialAlivePredatorCount;

    /// <summary>
    ///   The initial amount of alive prey.
    /// </summary>
    [SerializeField] private int initialAlivePreyCount;

    /// <summary>
    ///   The initial amount of rabbits.
    /// </summary>
    [SerializeField] private int initialAliveRabbitsCount;

    /// <summary>
    ///   The initial amount of deer.
    /// </summary>
    [SerializeField] private int initialAliveDeerCount;

    /// <summary>
    ///   The initial amount of wolves.
    /// </summary>
    [SerializeField] private int initialAliveWolvesCount;

    /// <summary>
    ///   The initial amount of bears.
    /// </summary>
    [SerializeField] private int initialAliveBearsCount;

    /// <summary>
    ///   The final amount of available food items.
    /// </summary>
    [SerializeField] private int foodCount;

    /// <summary>
    ///   The final amount of alive animals.
    /// </summary>
    [SerializeField] private int aliveCount;

    /// <summary>
    ///   The amount of animals that have died.
    /// </summary>
    [SerializeField] private int deadCount;

    /// <summary>
    ///   The amount of animals that have been born.
    /// </summary>
    [SerializeField] private int birthCount;

    /// <summary>
    ///   The amount of prey that were consumed.
    /// </summary>
    [SerializeField] private int preyConsumedCount;

    /// <summary>
    ///   The history of simulation events, stored in chronological order. 
    /// </summary>
    [SerializeField] private List<SimulationEvent> events = new List<SimulationEvent>();

    [SerializeField] private List<Death> deaths = new List<Death>();

    /// <summary>
    ///   Counts the current amount of animals and food sources. Used to determine the initial population sizes.
    /// </summary>
    /// <remarks>
    ///   The counting logic assumes that only the root objects of our prefabs feature the
    ///   identifying tags. If that wouldn't be the case, this approach would overestimate the amounts.
    /// </remarks>
    public void CountInitialStats()
    {
      initialAliveRabbitsCount = Tags.Count("Rabbit");
      initialAliveDeerCount = Tags.Count("Deer");
      initialAliveWolvesCount = Tags.Count("Wolf");
      initialAliveBearsCount = Tags.Count("Bear");

      initialAlivePredatorCount = Tags.CountPredators();
      initialAlivePreyCount = Tags.CountPrey();
      initialFoodCount = Tags.CountFood();
      initialAliveCount = initialAlivePreyCount + initialAlivePredatorCount;

      aliveCount = initialAliveCount;
      foodCount = initialFoodCount;
    }

    /// <summary>
    ///   Marks the simulation as finished. This is used to determine the duration of the simulation.
    /// </summary>
    public void MarkAsDone()
    {
      duration = SessionTime.Now();
    }


    /// <summary>
    ///   Adds a simulation event that represents the birth of an animal.
    /// </summary>
    /// <param name="animal">the game object associated with the animal that was born.</param>
    public void AddBirth(GameObject animal)
    {
      events.Add(new SimulationEvent
      {
              time = SessionTime.Now(),
              type = "birth",
              tag = animal.tag,
              position = animal.transform.position,
              matingIndex = -1,
              deathIndex = -1
      });

      ++birthCount;
      ++aliveCount;
    }

    /// <summary>
    ///   Adds a simulation event that represents the death of an animal.
    /// </summary>
    /// <param name="deadObject">the game object associated with the animal that died.</param>
    /// <param name="cause">the cause of death for the animal.</param>
    public void AddDeath(GameObject deadObject, CauseOfDeath cause)
    {
      events.Add(new SimulationEvent
      {
              time = SessionTime.Now(),
              type = "death",
              tag = deadObject.tag,
              position = deadObject.transform.position,
              matingIndex = -1,
              deathIndex = deaths.Count
      });

      deaths.Add(new Death
      {
              cause = cause
      });

      --aliveCount;
      ++deadCount;
    }

    /// <summary>
    ///   Adds a simulation event that represents a food item being consumed.
    /// </summary>
    /// <param name="food">the game object associated with the food item that was consumed.</param>
    public void AddConsumption(GameObject food)
    {
      events.Add(new SimulationEvent
      {
              time = SessionTime.Now(),
              type = "consumption",
              tag = food.tag,
              position = food.transform.position,
              matingIndex = -1,
              deathIndex = -1
      });

      --foodCount;
    }

    public void AddPreyConsumption()
    {
      // We only count the number of consumed prey, more information will be logged as 
      // a death event
      ++preyConsumedCount;
    }

    public int AliveCount() => aliveCount;

    public int DeadCount() => deadCount;

    public int BirthCount() => birthCount;

    public int FoodCount() => foodCount;

    public int PreyConsumedCount() => preyConsumedCount;
  }
}