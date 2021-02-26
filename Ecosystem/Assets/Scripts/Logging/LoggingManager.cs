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

      /*
       * The following counting logic assumes that only the root objects of our prefabs feature
       * the identifying tags. If that wouldn't be the case, this approach would overestimate the
       * amounts.
       */
      _data.aliveCount = GameObject.FindGameObjectsWithTag("Prey").Length +
                         GameObject.FindGameObjectsWithTag("Predator").Length;

      _data.foodCount = GameObject.FindGameObjectsWithTag("Food").Length;

      _data.initialAliveCount = _data.aliveCount;
      _data.initialFoodCount = _data.foodCount;

      aliveCountText.text = _data.aliveCount.ToString();
      foodCountText.text = _data.foodCount.ToString();

      preyConsumedCountText.text = "0";
      deadCountText.text = "0";
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

    private void LogDeath(CauseOfDeath cause, GameObject deadObject)
    {
      _data.deaths.Add(new Death
      {
              time = AnalyticsSessionInfo.sessionElapsedTime,
              cause = cause,
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