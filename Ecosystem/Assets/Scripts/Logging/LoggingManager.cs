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
  /// exit.
  /// </summary>
  public sealed class LoggingManager : MonoBehaviour
  {
    [SerializeField] private Text aliveCountText;
    [SerializeField] private Text deadCountText;
    [SerializeField] private Text foodCountText;
    [SerializeField] private Text preyConsumedCountText;
    [SerializeField] private Text timePassedText;

    private int _aliveCount;
    private int _deadCount;
    private int _foodCount;
    private int _preyConsumedCount;
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
      _aliveCount = GameObject.FindGameObjectsWithTag("Prey").Length +
                    GameObject.FindGameObjectsWithTag("Predator").Length;

      _foodCount = GameObject.FindGameObjectsWithTag("Food").Length;

      aliveCountText.text = _aliveCount.ToString();
      foodCountText.text = _foodCount.ToString();
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
    }

    private void LogDeath(CauseOfDeath cause, GameObject deadObject)
    {
      --_aliveCount;
      ++_deadCount;

      aliveCountText.text = _aliveCount.ToString();
      deadCountText.text = _deadCount.ToString();
    }

    private void LogFoodEaten()
    {
      --_foodCount;
      foodCountText.text = _foodCount.ToString();
    }

    private void LogPreyConsumed()
    {
      ++_preyConsumedCount;
      preyConsumedCountText.text = _preyConsumedCount.ToString();
    }
  }
}