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

    //TODO: make this not be saved.   
    /// <summary>
    ///   Ignore list, only for constructing the finished product (due to the immutability of serializable structs)
    /// </summary>
    [SerializeField] private List<GenomeInfo> workInProgressGenomes = new List<GenomeInfo>(64);

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

    private long freq = 1000; //1/sec

    //Because to many boxes are bad. 
    private int boxFreqFactor = 5;

    /// <summary>
    ///   Prepares the data with the initial simulation state. Used to determine the
    ///   initial population sizes, etc.
    /// </summary>
    public void PrepareData()
    {
      CaptureInitialGenomes();
    }

    private void CaptureInitialGenomes()
    {
      //breaks design principle.
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

    private int CaptureByTag(string tag)
    {
      int count = 0;
      GameObject[] animals = GameObject.FindGameObjectsWithTag(tag);
      foreach (var animal in animals)
      {
        if (animal.TryGetComponent(out AbstractGenome g))
        {
          count++;
          workInProgressGenomes.Add(new GenomeInfo()
          {
            endTime = -1,
            genes = GenomeDataToList(g.GetGenes()),
            key = g.key,
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
      workInProgressGenomes = new List<GenomeInfo>();
      AssignAverages();
      AssignBoxes();
    }

    private void AssignAverages()
    {
      rabbitAverageGenomes = new AverageGenomes()
      {
        animal = "Rabbit",
        HungerRate = CreateAverages("Rabbit", GeneType.HungerRate),
        HungerThreshold = CreateAverages("Rabbit", GeneType.HungerThreshold),
        ThirstRate = CreateAverages("Rabbit", GeneType.ThirstRate),
        ThirstThreshold = CreateAverages("Rabbit", GeneType.ThirstThreshold),
        Vision = CreateAverages("Rabbit", GeneType.Vision),
        Speed = CreateAverages("Rabbit", GeneType.Speed),
        SizeFactor = CreateAverages("Rabbit", GeneType.SizeFactor),
        DesirabilityScore = CreateAverages("Rabbit", GeneType.DesirabilityScore),
        GestationPeriod = CreateAverages("Rabbit", GeneType.GestationPeriod),
        SexualMaturityTime = CreateAverages("Rabbit", GeneType.SexualMaturityTime),
      };
      wolfAverageGenomes = new AverageGenomes()
      {
        animal = "Wolf",
        HungerRate = CreateAverages("Wolf", GeneType.HungerRate),
        HungerThreshold = CreateAverages("Wolf", GeneType.HungerThreshold),
        ThirstRate = CreateAverages("Wolf", GeneType.ThirstRate),
        ThirstThreshold = CreateAverages("Wolf", GeneType.ThirstThreshold),
        Vision = CreateAverages("Wolf", GeneType.Vision),
        Speed = CreateAverages("Wolf", GeneType.Speed),
        SizeFactor = CreateAverages("Wolf", GeneType.SizeFactor),
        DesirabilityScore = CreateAverages("Wolf", GeneType.DesirabilityScore),
        GestationPeriod = CreateAverages("Wolf", GeneType.GestationPeriod),
        SexualMaturityTime = CreateAverages("Wolf", GeneType.SexualMaturityTime),
      };
      deerAverageGenomes = new AverageGenomes()
      {
        animal = "Deer",
        HungerRate = CreateAverages("Deer", GeneType.HungerRate),
        HungerThreshold = CreateAverages("Deer", GeneType.HungerThreshold),
        ThirstRate = CreateAverages("Deer", GeneType.ThirstRate),
        ThirstThreshold = CreateAverages("Deer", GeneType.ThirstThreshold),
        Vision = CreateAverages("Deer", GeneType.Vision),
        Speed = CreateAverages("Deer", GeneType.Speed),
        SizeFactor = CreateAverages("Deer", GeneType.SizeFactor),
        DesirabilityScore = CreateAverages("Deer", GeneType.DesirabilityScore),
        GestationPeriod = CreateAverages("Deer", GeneType.GestationPeriod),
        SexualMaturityTime = CreateAverages("Deer", GeneType.SexualMaturityTime),
      };
      bearAverageGenomes = new AverageGenomes()
      {
        animal = "Bear",
        HungerRate = CreateAverages("Bear", GeneType.HungerRate),
        HungerThreshold = CreateAverages("Bear", GeneType.HungerThreshold),
        ThirstRate = CreateAverages("Bear", GeneType.ThirstRate),
        ThirstThreshold = CreateAverages("Bear", GeneType.ThirstThreshold),
        Vision = CreateAverages("Bear", GeneType.Vision),
        Speed = CreateAverages("Bear", GeneType.Speed),
        SizeFactor = CreateAverages("Bear", GeneType.SizeFactor),
        DesirabilityScore = CreateAverages("Bear", GeneType.DesirabilityScore),
        GestationPeriod = CreateAverages("Bear", GeneType.GestationPeriod),
        SexualMaturityTime = CreateAverages("Bear", GeneType.SexualMaturityTime),
      };
    }

    private void AssignBoxes()
    {
      rabbitBoxGenomes = new BoxGenomes()
      {
        animal = "Rabbit",
        HungerRate = CreateBoxes("Rabbit", GeneType.HungerRate),
        HungerThreshold = CreateBoxes("Rabbit", GeneType.HungerThreshold),
        ThirstRate = CreateBoxes("Rabbit", GeneType.ThirstRate),
        ThirstThreshold = CreateBoxes("Rabbit", GeneType.ThirstThreshold),
        Vision = CreateBoxes("Rabbit", GeneType.Vision),
        Speed = CreateBoxes("Rabbit", GeneType.Speed),
        SizeFactor = CreateBoxes("Rabbit", GeneType.SizeFactor),
        DesirabilityScore = CreateBoxes("Rabbit", GeneType.DesirabilityScore),
        GestationPeriod = CreateBoxes("Rabbit", GeneType.GestationPeriod),
        SexualMaturityTime = CreateBoxes("Rabbit", GeneType.SexualMaturityTime),
      };
      wolfBoxGenomes = new BoxGenomes()
      {
        animal = "Wolf",
        HungerRate = CreateBoxes("Wolf", GeneType.HungerRate),
        HungerThreshold = CreateBoxes("Wolf", GeneType.HungerThreshold),
        ThirstRate = CreateBoxes("Wolf", GeneType.ThirstRate),
        ThirstThreshold = CreateBoxes("Wolf", GeneType.ThirstThreshold),
        Vision = CreateBoxes("Wolf", GeneType.Vision),
        Speed = CreateBoxes("Wolf", GeneType.Speed),
        SizeFactor = CreateBoxes("Wolf", GeneType.SizeFactor),
        DesirabilityScore = CreateBoxes("Wolf", GeneType.DesirabilityScore),
        GestationPeriod = CreateBoxes("Wolf", GeneType.GestationPeriod),
        SexualMaturityTime = CreateBoxes("Wolf", GeneType.SexualMaturityTime),
      };
      deerBoxGenomes = new BoxGenomes()
      {
        animal = "Deer",
        HungerRate = CreateBoxes("Deer", GeneType.HungerRate),
        HungerThreshold = CreateBoxes("Deer", GeneType.HungerThreshold),
        ThirstRate = CreateBoxes("Deer", GeneType.ThirstRate),
        ThirstThreshold = CreateBoxes("Deer", GeneType.ThirstThreshold),
        Vision = CreateBoxes("Deer", GeneType.Vision),
        Speed = CreateBoxes("Deer", GeneType.Speed),
        SizeFactor = CreateBoxes("Deer", GeneType.SizeFactor),
        DesirabilityScore = CreateBoxes("Deer", GeneType.DesirabilityScore),
        GestationPeriod = CreateBoxes("Deer", GeneType.GestationPeriod),
        SexualMaturityTime = CreateBoxes("Deer", GeneType.SexualMaturityTime),
      };
      bearBoxGenomes = new BoxGenomes()
      {
        animal = "Bear",
        HungerRate = CreateBoxes("Bear", GeneType.HungerRate),
        HungerThreshold = CreateBoxes("Bear", GeneType.HungerThreshold),
        ThirstRate = CreateBoxes("Bear", GeneType.ThirstRate),
        ThirstThreshold = CreateBoxes("Bear", GeneType.ThirstThreshold),
        Vision = CreateBoxes("Bear", GeneType.Vision),
        Speed = CreateBoxes("Bear", GeneType.Speed),
        SizeFactor = CreateBoxes("Bear", GeneType.SizeFactor),
        DesirabilityScore = CreateBoxes("Bear", GeneType.DesirabilityScore),
        GestationPeriod = CreateBoxes("Bear", GeneType.GestationPeriod),
        SexualMaturityTime = CreateBoxes("Bear", GeneType.SexualMaturityTime),
      };
    }

    private List<GeneAverageInfo> CreateAverages(string tag, GeneType type)
    {
      List<GeneAverageInfo> averages = new List<GeneAverageInfo>();
      var animalGenomes = genomes.FindAll(genome => genome.tag.Equals(tag));
      for (long i = 0; i < duration; i += freq)
      {
        //Find all genomes active during that period
        List<GenomeInfo> currentGI = animalGenomes.FindAll(g => i >= g.time && i <= g.endTime);
        //only care if some values exist. 
        if (currentGI.Count > 0)
        {
          List<float> vals = new List<float>();
          /*
          foreach (var gi in currentGI)
          {
            vals.Add(gi.genes.Find(g => g.geneType.Equals(type)).value);
          }
          */
          currentGI.ForEach(genomeInfo =>
            vals.Add(genomeInfo.genes.Find(geneInfo => geneInfo.geneType.Equals(type)).value));

          averages.Add(new GeneAverageInfo()
          {
            entryTime = i,
            value = vals.Sum() / vals.Count
          });
        }
      }

      return averages;
    }

    private List<GeneBoxInfo> CreateBoxes(string tag, GeneType type)
    {
      var animalGenomes = genomes.FindAll(genome => genome.tag.Equals(tag));
      List<GeneBoxInfo> box = new List<GeneBoxInfo>();
      for (long i = 0; i < duration; i += (freq * boxFreqFactor))
      {
        //Find all genomes active during that period
        List<GenomeInfo> currentGI = animalGenomes.FindAll(g => i >= g.time && i <= g.endTime);
        //only care if some values exist. 
        if (currentGI.Count > 0)
        {
          List<float> vals = new List<float>();
          /*
          foreach (var gi in currentGI)
          {
            vals.Add(gi.genes.Find(g => g.geneType.Equals(type)).value);
          }
          */
          currentGI.ForEach(genomeInfo =>
            vals.Add(genomeInfo.genes.Find(geneInfo => geneInfo.geneType.Equals(type)).value));
          box.Add(new GeneBoxInfo()
          {
            entryTime = i,
            value = vals
          });
        }
      }

      return box;
    }

    private void MatchGenomeToTime()
    {
      foreach (var pair in keyEnd)
      {
        GenomeInfo genomeInfo = workInProgressGenomes.Find(genome => genome.key.Equals(pair.Key));
        workInProgressGenomes.Remove(genomeInfo);
        genomes.Add(new GenomeInfo()
        {
          tag = genomeInfo.tag,
          endTime = pair.Value,
          genes = genomeInfo.genes,
          key = genomeInfo.key,
          time = genomeInfo.time
        });
      }

      foreach (var genome in workInProgressGenomes)
      {
        //Find unended/unadded genomes
        if (genome.endTime.Equals(-1))
        {
          genomes.Add(new GenomeInfo()
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

      AbstractGenome abstractGenome = animal.GetComponent<AbstractGenome>();
      workInProgressGenomes.Add(new GenomeInfo()
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

      string key = deadObject.GetComponent<AbstractGenome>().key;
      try
      {
        keyEnd.Add(key, SessionTime.Now());
      }
      catch
      {
        //animals dying multiple times, causing warning messages. If here, animal already died once.
      }
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
      CreateGeneInfo(GeneType.SizeFactor, genome.GetSizeFactor()),
      CreateGeneInfo(GeneType.DesirabilityScore, genome.GetDesirabilityScore()),
      CreateGeneInfo(GeneType.GestationPeriod, genome.GetGestationPeriod()),
      CreateGeneInfo(GeneType.SexualMaturityTime, genome.GetSexualMaturityTime())
    };

    private static GeneInfo CreateGeneInfo(GeneType type, Gene gene) => new GeneInfo
    {
      geneType = type,
      value = gene.Value
    };

    private static List<GeneInfo> GenomeDataToList(GenomeData data)
    {
      return new List<GeneInfo>()
      {
        new GeneInfo()
        {
          geneType = GeneType.HungerRate,
          value = data.HungerRate.Value
        },
        new GeneInfo()
        {
          geneType = GeneType.HungerThreshold,
          value = data.HungerThreshold.Value
        },
        new GeneInfo()
        {
          geneType = GeneType.ThirstRate,
          value = data.ThirstRate.Value
        },
        new GeneInfo()
        {
          geneType = GeneType.ThirstThreshold,
          value = data.ThirstThreshold.Value
        },
        new GeneInfo()
        {
          geneType = GeneType.Vision,
          value = data.Vision.Value
        },
        new GeneInfo()
        {
          geneType = GeneType.Speed,
          value = data.Speed.Value
        },
        new GeneInfo()
        {
          geneType = GeneType.SizeFactor,
          value = data.SizeFactor.Value
        },
        new GeneInfo()
        {
          geneType = GeneType.DesirabilityScore,
          value = data.DesirabilityScore.Value
        },
        new GeneInfo()
        {
          geneType = GeneType.GestationPeriod,
          value = data.GestationPeriod.Value
        },
        new GeneInfo()
        {
          geneType = GeneType.SexualMaturityTime,
          value = data.SexualMaturityTime.Value
        }
      };
    }
  }
}