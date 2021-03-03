using UnityEngine;
using UnityEngine.Analytics;
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
    [SerializeField] private Text preyConsumedCountText;
    [SerializeField] private Text timePassedText;

    private readonly LogData _data = new LogData();
    private long _nextUpdateTime;

    private void Start()
    {
      // Yes, these are allocated once, it's fine
      DeathHandler.OnDeath += LogDeath;
      FoodConsumer.OnFoodEaten += LogFoodEaten;
      PreyConsumer.OnPreyConsumed += LogPreyConsumed;
      Reproducer.OnBirth += LogBirth;

      /*
       * The following counting logic assumes that only the root objects of our prefabs feature
       * the identifying tags. If that wouldn't be the case, this approach would overestimate the
       * amounts.
       */

      _data.initialAlivePredatorCount = GameObject.FindGameObjectsWithTag("Predator").Length;
      _data.initialAlivePreyCount = GameObject.FindGameObjectsWithTag("Prey").Length;

      _data.initialAliveCount = _data.initialAlivePreyCount + _data.initialAlivePredatorCount;
      _data.aliveCount = _data.initialAliveCount;

      _data.initialFoodCount = GameObject.FindGameObjectsWithTag("Food").Length;
      _data.foodCount = _data.initialFoodCount;

      aliveCountText.text = _data.aliveCount.ToString();
      foodCountText.text = _data.foodCount.ToString();

      birthCountText.text = "0";
      deadCountText.text = "0";
      preyConsumedCountText.text = "0";
    }

    private void Update()
    {
      var milliseconds = AnalyticsSessionInfo.sessionElapsedTime;
      if (milliseconds > _nextUpdateTime)
      {
        var seconds = milliseconds / 1_000;
        timePassedText.text = seconds.ToString();
        _nextUpdateTime = milliseconds + 1_000;
      }
    }

    private void OnApplicationQuit()
    {
      _data.duration = AnalyticsSessionInfo.sessionElapsedTime;
      LogFileWriter.Save(_data);
    }

    private void LogBirth(GameObject animal)
    {
      _data.births.Add(new Birth
      {
              time = AnalyticsSessionInfo.sessionElapsedTime,
              tag = animal.tag,
              position = animal.transform.position
      });

      birthCountText.text = _data.births.Count.ToString();
    }

    private void LogDeath(CauseOfDeath cause, GameObject deadObject)
    {
      _data.deaths.Add(new Death
      {
              time = AnalyticsSessionInfo.sessionElapsedTime,
              cause = cause,
              tag = deadObject.tag,
              position = deadObject.transform.position
      });

      --_data.aliveCount;
      ++_data.deadCount;

      aliveCountText.text = _data.aliveCount.ToString();
      deadCountText.text = _data.deadCount.ToString();
    }

    private void LogFoodEaten(GameObject food)
    {
      _data.foodConsumptions.Add(new FoodConsumption
      {
              time = AnalyticsSessionInfo.sessionElapsedTime,
              position = food.transform.position
      });

      --_data.foodCount;
      foodCountText.text = _data.foodCount.ToString();
    }

    private void LogPreyConsumed()
    {
      // We only count the number of consumed prey, more information will be logged as 
      // a death event
      ++_data.preyConsumedCount;
      preyConsumedCountText.text = _data.preyConsumedCount.ToString();
    }
  }
}