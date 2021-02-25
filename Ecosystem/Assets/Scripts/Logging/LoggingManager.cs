using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

namespace Ecosystem.Logging
{
  public sealed class LoggingManager : MonoBehaviour
  {
    [SerializeField] private Text aliveCountText;
    [SerializeField] private Text deadCountText;
    [SerializeField] private Text foodCountText;
    [SerializeField] private Text timePassedText;

    private int _aliveCount;
    private int _deadCount;
    private int _foodCount;
    private long _nextUpdateTime;

    private void Start()
    {
      // Yes, these are allocated once, it's fine
      DeathHandler.OnDeath += LogDeath;
      FoodConsumer.OnFoodEaten += LogFoodEaten;

      /*
       * The following counting logic assumes that only the root objects of our prefabs feature
       * the identifying tags. If that wouldn't be the case, this approach will overestimate the amounts.
       */

      foreach (var unused in GameObject.FindGameObjectsWithTag("Prey"))
      {
        ++_aliveCount;
      }

      foreach (var unused in GameObject.FindGameObjectsWithTag("Predator"))
      {
        ++_aliveCount;
      }

      foreach (var unused in GameObject.FindGameObjectsWithTag("Food"))
      {
        ++_foodCount;
      }

      aliveCountText.text = _aliveCount.ToString();
      foodCountText.text = _foodCount.ToString();
      deadCountText.text = "0";
    }

    private void Update()
    {
      var now = AnalyticsSessionInfo.sessionElapsedTime;
      if (now > _nextUpdateTime)
      {
        var seconds = now / 1_000;
        timePassedText.text = seconds.ToString();
        _nextUpdateTime = now + 1_000; // Update time label once every second
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
  }
}