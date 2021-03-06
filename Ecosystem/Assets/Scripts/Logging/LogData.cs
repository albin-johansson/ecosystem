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
    ///   The amount of matings.
    /// </summary>
    [SerializeField] private int matingCount;

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

    /// <summary>
    ///   Additional information about mating events.
    /// </summary>
    [SerializeField] private List<Mating> matings = new List<Mating>();

    /// <summary>
    ///   Additional information about death events.
    /// </summary>
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
    ///   Adds a simulation event that represents the mating of two animals.
    /// </summary>
    /// <param name="position">the position where the animals mated.</param>
    /// <param name="animalTag">the tag associated with the animals.</param>
    /// <param name="male">the genome associated with the male.</param>
    /// <param name="female">the genome associated with the female.</param>
    public void AddMating(Vector3 position, string animalTag, Genome male, Genome female)
    {
      events.Add(new SimulationEvent
      {
              time = SessionTime.Now(),
              type = "mating",
              tag = animalTag,
              position = position,
              matingIndex = matings.Count,
              deathIndex = -1
      });

      var info = new Mating
      {
              male = new List<GeneInfo>(),
              female = new List<GeneInfo>()
      };

      foreach (var pair in male.Genes)
      {
        AddGene(info.male, pair);
      }

      foreach (var pair in female.Genes)
      {
        AddGene(info.female, pair);
      }

      matings.Add(info);
      ++matingCount;
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

    /// <summary>
    ///   Increments the count of consumed prey.
    /// </summary>
    /// <remarks>
    ///   This isn't recorded as a simulation event, more information is meant to be logged
    ///   in the associated death event. 
    /// </remarks>
    public void AddPreyConsumption()
    {
      ++preyConsumedCount;
    }

    /// <summary>
    ///   Returns the current count of alive animals.
    /// </summary>
    /// <returns>the amount of alive animals.</returns>
    public int AliveCount() => aliveCount;

    /// <summary>
    ///   Returns the current count of dead animals.
    /// </summary>
    /// <returns>the amount of dead animals.</returns>
    public int DeadCount() => deadCount;

    /// <summary>
    ///   Returns the current count of born animals.
    /// </summary>
    /// <returns>the amount of born animals.</returns>
    public int BirthCount() => birthCount;

    /// <summary>
    ///   Returns the current count of food items.
    /// </summary>
    /// <returns>the amount of food items.</returns>
    public int FoodCount() => foodCount;

    /// <summary>
    ///   Returns the current count of matings.
    /// </summary>
    /// <returns>the amount of matings.</returns>
    public int MatingCount() => matingCount;

    /// <summary>
    ///   Returns the current count consumed prey.
    /// </summary>
    /// <returns>the amount of consumed prey.</returns>
    public int PreyConsumedCount() => preyConsumedCount;

    private static void AddGene(ICollection<GeneInfo> genes, KeyValuePair<GeneType, Gene> pair)
    {
      var gene = pair.Value;
      genes.Add(new GeneInfo
      {
              type = pair.Key,
              min = gene.Min,
              max = gene.Max,
              value = gene.Value
      });
    }
  }
}