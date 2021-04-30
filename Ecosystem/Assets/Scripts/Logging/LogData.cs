using System;
using System.Collections.Generic;
using System.Linq;
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
    private static readonly int GeneCount = Enum.GetValues(typeof(GeneType)).Length;

    #region Fields

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
    ///   List of the genomes that count
    /// </summary>
    [SerializeField] private List<GenomeInfo> genomes = new List<GenomeInfo>(64);

    /// <summary>
    ///   Maps death of genome by key to the death time. 
    /// </summary>
    [SerializeField] private Dictionary<string, long> keyEnd = new Dictionary<string, long>();

    //   Saved averages for python 
    [SerializeField] private AverageGenomes rabbitAverageGenomes;
    [SerializeField] private AverageGenomes wolfAverageGenomes;
    [SerializeField] private AverageGenomes deerAverageGenomes;
    [SerializeField] private AverageGenomes bearAverageGenomes;

    //   Saved box spreads for python
    [SerializeField] private BoxGenomes rabbitBoxGenomes;
    [SerializeField] private BoxGenomes wolfBoxGenomes;
    [SerializeField] private BoxGenomes deerBoxGenomes;
    [SerializeField] private BoxGenomes bearBoxGenomes;

    [SerializeField] private long freq = 1000; // 1 update per second
    [SerializeField] private long boxFreqFactor; // Because to many boxes are bad. 

    /// Only used for constructing the finished product (due to the immutability of serializable structs)
    [NonSerialized] private List<GenomeInfo> _workInProgressGenomes = new List<GenomeInfo>(64);

    #endregion

    /// <summary>
    ///   Prepares the data with the initial simulation state. Used to determine the
    ///   initial population sizes, etc.
    /// </summary>
    public void PrepareData()
    {
      CaptureInitialGenomes();
    }

    private int CaptureByTag(string tag)
    {
      var count = 0;
      foreach (var animal in GameObject.FindGameObjectsWithTag(tag))
      {
        if (animal.TryGetComponent(out AbstractGenome genome))
        {
          ++count;
          _workInProgressGenomes.Add(new GenomeInfo()
          {
            endTime = -1,
            genes = GenomeDataToList(genome.GetGenes()),
            key = genome.key,
            tag = tag,
            time = 0
          });
        }
      }

      return count;
    }

    /// <summary>
    ///   Marks the simulation as finished. This is used to determine the duration of the simulation.
    /// </summary>
    public void MarkAsDone()
    {
      duration = SessionTime.Now();
      MatchGenomeToTime();
      _workInProgressGenomes = new List<GenomeInfo>();
      boxFreqFactor = 2; //might need changes if simulation is too long.  
      AssignAverages();
      AssignBoxes();
    }

    private AverageGenomes CreateAverageGenomes(string animal) => new AverageGenomes
    {
      animal = animal,
      HungerRate = CreateAverages(animal, GeneType.HungerRate),
      HungerThreshold = CreateAverages(animal, GeneType.HungerThreshold),
      ThirstRate = CreateAverages(animal, GeneType.ThirstRate),
      ThirstThreshold = CreateAverages(animal, GeneType.ThirstThreshold),
      Vision = CreateAverages(animal, GeneType.Vision),
      Speed = CreateAverages(animal, GeneType.Speed),
      GestationPeriod = CreateAverages(animal, GeneType.GestationPeriod),
      SexualMaturityTime = CreateAverages(animal, GeneType.SexualMaturityTime),
    };

    private BoxGenomes CreateBoxGenomes(string animal) => new BoxGenomes
    {
      animal = animal,
      HungerRate = CreateBoxes(animal, GeneType.HungerRate),
      HungerThreshold = CreateBoxes(animal, GeneType.HungerThreshold),
      ThirstRate = CreateBoxes(animal, GeneType.ThirstRate),
      ThirstThreshold = CreateBoxes(animal, GeneType.ThirstThreshold),
      Vision = CreateBoxes(animal, GeneType.Vision),
      Speed = CreateBoxes(animal, GeneType.Speed),
      GestationPeriod = CreateBoxes(animal, GeneType.GestationPeriod),
      SexualMaturityTime = CreateBoxes(animal, GeneType.SexualMaturityTime),
    };

    private void AssignAverages()
    {
      rabbitAverageGenomes = CreateAverageGenomes("Rabbit");
      deerAverageGenomes = CreateAverageGenomes("Deer");
      wolfAverageGenomes = CreateAverageGenomes("Wolf");
      bearAverageGenomes = CreateAverageGenomes("Bear");
    }

    private void AssignBoxes()
    {
      rabbitBoxGenomes = CreateBoxGenomes("Rabbit");
      deerBoxGenomes = CreateBoxGenomes("Deer");
      wolfBoxGenomes = CreateBoxGenomes("Wolf");
      bearBoxGenomes = CreateBoxGenomes("Bear");
    }

    private void ForEachInfo(string tag, GeneType type, long freqFactor, Action<long, List<float>> action)
    {
      var animalGenomes = genomes.FindAll(genome => genome.tag.Equals(tag));
      for (long time = 0; time < duration; time += freq * freqFactor)
      {
        // Find all genomes active during that period
        var infos = animalGenomes.FindAll(info => time >= info.time && time <= info.endTime);

        // Only care if some values exist. 
        if (infos.Count > 0)
        {
          var values = new List<float>();

          infos.ForEach(info => values.Add(info.genes.Find(otherInfo => otherInfo.geneType == type).value));

          action.Invoke(time, values);
        }
      }
    }

    private List<GeneAverageInfo> CreateAverages(string tag, GeneType type)
    {
      var averages = new List<GeneAverageInfo>();

      ForEachInfo(tag, type, 1, (time, values) => averages.Add(new GeneAverageInfo
      {
        entryTime = time,
        value = values.Sum() / values.Count
      }));

      return averages;
    }

    private List<GeneBoxInfo> CreateBoxes(string tag, GeneType type)
    {
      var box = new List<GeneBoxInfo>();

      ForEachInfo(tag, type, boxFreqFactor, (time, values) => box.Add(new GeneBoxInfo
      {
        entryTime = time,
        value = values
      }));

      return box;
    }

    private void MatchGenomeToTime()
    {
      foreach (var pair in keyEnd)
      {
        var genomeInfo = _workInProgressGenomes.Find(genome => genome.key.Equals(pair.Key));
        _workInProgressGenomes.Remove(genomeInfo);
        genomes.Add(new GenomeInfo
        {
          tag = genomeInfo.tag,
          endTime = pair.Value,
          genes = genomeInfo.genes,
          key = genomeInfo.key,
          time = genomeInfo.time
        });
      }

      foreach (var genome in _workInProgressGenomes)
      {
        // Find unended/unadded genomes
        if (genome.endTime.Equals(-1))
        {
          genomes.Add(new GenomeInfo
          {
            tag = genome.tag,
            endTime = duration,
            genes = genome.genes,
            key = genome.key,
            time = genome.time
          });
        }
      }
    }

    #region Event recording functions

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

      var abstractGenome = animal.GetComponent<AbstractGenome>();
      _workInProgressGenomes.Add(new GenomeInfo
      {
        tag = animal.tag,
        time = SessionTime.Now(),
        endTime = -1,
        key = abstractGenome.key,
        genes = GenomeDataToList(abstractGenome.Data)
      });
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

      var key = deadObject.GetComponent<AbstractGenome>().key;

      // Animals die multiple times :/. So the key might already exist.
      if (!keyEnd.ContainsKey(key))
      {
        keyEnd.Add(key, SessionTime.Now());
      }
    }

    /// <summary>
    ///   Adds a simulation event that represents a food item being consumed.
    /// </summary>
    /// <param name="food">the game object associated with the food item that was consumed.</param>
    public void AddFoodConsumption(GameObject food)
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
    ///   Adds a simulation event that represents a food item decaying.
    /// </summary>
    /// <param name="food">the game object associated with the food item that decayed.</param>
    public void AddFoodDecayed(GameObject food)
    {
      events.Add(new SimulationEvent
      {
        time = SessionTime.Now(),
        type = "food_decay",
        tag = food.tag,
        position = food.transform.position,
        matingIndex = -1,
        deathIndex = -1
      });

      --foodCount;
    }

    /// <summary>
    ///   Adds a simulation event that represents a food item being generated.
    /// </summary>
    /// <param name="food">the game object associated with the food item that was generated.</param>
    public void AddFoodGeneration(GameObject food)
    {
      events.Add(new SimulationEvent
      {
        time = SessionTime.Now(),
        type = "food_generation",
        tag = food.tag,
        position = food.transform.position,
        matingIndex = -1,
        deathIndex = -1
      });

      ++foodCount;
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

    #endregion

    #region Public count queries

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

    #endregion

    #region Genome functions

    private void CaptureInitialGenomes()
    {
      initialAliveRabbitsCount = CaptureByTag("Rabbit");
      initialAliveDeerCount = CaptureByTag("Deer");
      initialAliveWolvesCount = CaptureByTag("Wolf");
      initialAliveBearsCount = CaptureByTag("Bear");
      initialAlivePredatorCount = initialAliveBearsCount + initialAliveWolvesCount;
      initialAlivePreyCount = initialAliveRabbitsCount + initialAliveDeerCount;
      initialAliveCount = initialAlivePredatorCount + initialAlivePreyCount;

      initialFoodCount = Tags.CountFood();

      aliveCount = initialAliveCount;
      foodCount = initialFoodCount;
    }

    private GenomeInfo CaptureGenome(Dictionary<GeneType, Gene> genes)
    {
      var info = new GenomeInfo {genes = new List<GeneInfo>()};

      foreach (var pair in genes)
      {
        info.genes.Add(CreateGeneInfo(pair.Key, pair.Value));
      }

      return info;
    }

    //not used, remove?
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
      CreateGeneInfo(GeneType.GestationPeriod, genome.GetGestationPeriod()),
      CreateGeneInfo(GeneType.SexualMaturityTime, genome.GetSexualMaturityTime())
    };

    private static GeneInfo CreateGeneInfo(GeneType type, Gene gene) => new GeneInfo
    {
      geneType = type,
      value = gene.Value
    };

    #endregion

    private static List<GeneInfo> GenomeDataToList(GenomeData data) => new List<GeneInfo>
    {
      new GeneInfo
      {
        geneType = GeneType.HungerRate,
        value = data.HungerRate.Value
      },
      new GeneInfo
      {
        geneType = GeneType.HungerThreshold,
        value = data.HungerThreshold.Value
      },
      new GeneInfo
      {
        geneType = GeneType.ThirstRate,
        value = data.ThirstRate.Value
      },
      new GeneInfo
      {
        geneType = GeneType.ThirstThreshold,
        value = data.ThirstThreshold.Value
      },
      new GeneInfo
      {
        geneType = GeneType.Vision,
        value = data.Vision.Value
      },
      new GeneInfo
      {
        geneType = GeneType.Speed,
        value = data.Speed.Value
      },
      new GeneInfo
      {
        geneType = GeneType.GestationPeriod,
        value = data.GestationPeriod.Value
      },
      new GeneInfo
      {
        geneType = GeneType.SexualMaturityTime,
        value = data.SexualMaturityTime.Value
      }
    };
  }
}