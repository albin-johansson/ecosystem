using Ecosystem.Spawning;
using Ecosystem.Consumer;
using Ecosystem.Genes;
using Ecosystem.UI;
using Ecosystem.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.Logging
{
  /// <summary>
  /// This class is responsible for logging the state of the simulation.
  /// The intention is that this is assigned to a root game object in the scene,
  /// and simply connected to the associated HUD elements. It will store the simulation
  /// data in-memory during the entire simulation, then dump the data to a file upon
  /// exit. This class keeps track of the simulation state by subscribing to events
  /// from various other scripts.
  /// </summary>
  public sealed class LoggingManager : MonoBehaviour
  {
    [SerializeField] private Text aliveCountText;
    [SerializeField] private Text birthCountText;
    [SerializeField] private Text deadCountText;
    [SerializeField] private Text foodCountText;
    [SerializeField] private Text matingCountText;
    [SerializeField] private Text preyConsumedCountText;
    [SerializeField] private Text timePassedText;

    private readonly LogData _data = new LogData();
    private long _nextUpdateTime;
    private long _startTime;

    private void Start()
    {
      _startTime = SessionTime.Now();
      SessionTime.SetSceneStart(_startTime);

      // Yes, these are allocated once, it's fine
      DeathHandler.OnDeath += LogDeath;

      // Food consumption
      NutritionController.OnFoodEaten += LogFoodEaten;
      StationaryFoodGeneration.OnFoodEaten += LogFoodEaten;

      // Prey consumption
      WolfConsumer.OnPreyConsumed += LogPreyConsumed;
      BearConsumer.OnPreyConsumed += LogPreyConsumed;

      // Mating and births
      Reproducer.OnBirth += LogBirth;
      Reproducer.OnMating += LogMating;

      // Food generation
      StationaryFoodGeneration.OnGeneratedFood += LogFoodGenerated;
      RadiusPrefabSpawner.OnGeneratedFood += LogFoodGenerated;
      RayPrefabSpawner.OnGeneratedFood += LogFoodGenerated;

      // Food decay
      NutritionController.OnFoodDecayed += LogFoodDecayed;

      _data.PrepareData();

      aliveCountText.text = _data.AliveCount().ToString();
      foodCountText.text = _data.FoodCount().ToString();
    }

    private void Update()
    {
      var milliseconds = SessionTime.NowSinceSceneStart();
      if (milliseconds > _nextUpdateTime)
      {
        var seconds = milliseconds / 1_000;
        timePassedText.text = seconds.ToString();
        _nextUpdateTime = milliseconds + 1_000;
      }
    }
    
    private void OnApplicationQuit()
    {
      _data.MarkAsDone();

      _data.SetMinFPS(FPSCounter.GetMinimumFPS());
      _data.SetMaxFPS(FPSCounter.GetMaximumFPS());
      _data.SetAverageFPS(FPSCounter.GetAverageFPS());

      LogFileWriter.Save(_data);
    }

    private void LogBirth(GameObject animal)
    {
      _data.AddBirth(animal);

      aliveCountText.text = _data.AliveCount().ToString();
      birthCountText.text = _data.BirthCount().ToString();
    }

    private void LogMating(Vector3 position, string animalTag, IGenome male, IGenome female)
    {
      _data.AddMating(position, animalTag, male, female);

      matingCountText.text = _data.MatingCount().ToString();
    }

    private void LogDeath(CauseOfDeath cause, GameObject deadObject)
    {
      _data.AddDeath(deadObject, cause);

      aliveCountText.text = _data.AliveCount().ToString();
      deadCountText.text = _data.DeadCount().ToString();
    }

    private void LogFoodEaten(GameObject food)
    {
      _data.AddFoodConsumption(food);
      foodCountText.text = _data.FoodCount().ToString();
    }

    private void LogFoodDecayed(GameObject food)
    {
      _data.AddFoodDecayed(food);
      foodCountText.text = _data.FoodCount().ToString();
    }

    private void LogFoodGenerated(GameObject food)
    {
      _data.AddFoodGeneration(food);
      foodCountText.text = _data.FoodCount().ToString();
    }

    private void LogPreyConsumed()
    {
      _data.AddPreyConsumption();
      preyConsumedCountText.text = _data.PreyConsumedCount().ToString();
    }
  }
}
