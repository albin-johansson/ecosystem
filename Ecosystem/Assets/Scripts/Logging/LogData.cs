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
  public sealed partial class LogData
  {
    private static readonly int GeneCount = Enum.GetValues(typeof(GeneType)).Length;

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
    ///   The base rabbit genome.
    /// </summary>
    [SerializeField] private GenomeInfo rabbitGenome;

    /// <summary>
    ///   The base deer genome.
    /// </summary>
    [SerializeField] private GenomeInfo deerGenome;

    /// <summary>
    ///   The base wolf genome.
    /// </summary>
    [SerializeField] private GenomeInfo wolfGenome;

    /// <summary>
    ///   The base bear genome.
    /// </summary>
    [SerializeField] private GenomeInfo bearGenome;

    /// <summary>
    ///   The history of simulation events, stored in chronological order. 
    /// </summary>
    [SerializeField] private List<SimulationEvent> events = new List<SimulationEvent>(256);

    /// <summary>
    ///   Additional information about mating events.
    /// </summary>
    [SerializeField] private List<Mating> matings = new List<Mating>(64);

    /// <summary>
    ///   Additional information about death events.
    /// </summary>
    [SerializeField] private List<Death> deaths = new List<Death>(64);

    /// <summary>
    ///   Prepares the data with the initial simulation state. Used to determine the
    ///   initial population sizes, etc.
    /// </summary>
    /// <remarks>
    ///   The counting logic assumes that only the root objects of our prefabs feature the
    ///   identifying tags. If that wouldn't be the case, this approach would overestimate the amounts.
    /// </remarks>
    public void PrepareData()
    {
      CountPopulationSizes();
      CaptureInitialGenomes();
    }

    private void CountPopulationSizes()
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

    private void CaptureInitialGenomes()
    {
      rabbitGenome = CaptureGenome(RabbitGenome.DefaultGenes);
      deerGenome = CaptureGenome(DeerGenome.DefaultGenes);
      wolfGenome = CaptureGenome(WolfGenome.DefaultGenes);
      bearGenome = CaptureGenome(BearGenome.DefaultGenes);
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
    public void AddMating(Vector3 position, string animalTag, IGenome male, IGenome female)
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

      matings.Add(CreateMatingInfo(male, female));
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

    private GenomeInfo CaptureGenome(Dictionary<GeneType, Gene> genes)
    {
      var info = new GenomeInfo {genes = new List<GeneInfo>()};

      foreach (var pair in genes)
      {
        info.genes.Add(CreateGeneInfo(pair.Key, pair.Value));
      }

      return info;
    }

    private static void AddGene(ICollection<GeneInfo> genes, KeyValuePair<GeneType, Gene> pair)
    {
      genes.Add(CreateGeneInfo(pair.Key, pair.Value));
    }

    private static Mating CreateMatingInfo(IGenome male, IGenome female) => new Mating
    {
      male = RecordGenes(male),
      female = RecordGenes(female)
    };

    private static List<GeneInfo> RecordGenes(IGenome genome) => new List<GeneInfo>(GeneCount)
    {
      CreateGeneInfo(GeneType.HungerRate, genome.GetHungerRate()),
      CreateGeneInfo(GeneType.HungerThreshold, genome.GetHungerThreshold()),
      CreateGeneInfo(GeneType.ThirstRate, genome.GetThirstRate()),
      CreateGeneInfo(GeneType.ThirstThreshold, genome.GetThirstThreshold()),
      CreateGeneInfo(GeneType.Vision, genome.GetVision()),
      CreateGeneInfo(GeneType.Speed, genome.GetSpeed()),
      CreateGeneInfo(GeneType.SizeFactor, genome.GetSizeFactor()),
      CreateGeneInfo(GeneType.DesirabilityScore, genome.GetDesirabilityScore()),
      CreateGeneInfo(GeneType.GestationPeriod, genome.GetGestationPeriod()),
      CreateGeneInfo(GeneType.SexualMaturityTime, genome.GetSexualMaturityTime())
    };

    private static GeneInfo CreateGeneInfo(GeneType type, Gene gene) => new GeneInfo
    {
      gene = type,
      value = gene.Value
    };
  }
}